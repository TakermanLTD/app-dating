using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Models.Enum;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;
using Takerman.Dating.Services.FacebookAuthentication;
using Takerman.Dating.Services.GoogleAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;
using static Google.Apis.Requests.BatchRequest;


namespace Takerman.Dating.Services
{
    public class UserService(IOptions<AppSettings> _appSettings, DefaultContext _context, IGoogleAuthService _googleAuthService, IFacebookAuthService _facebookAuthService, UserManager<User> _userManager, IOptions<Jwt> _jwt) : IUserService
    {

        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, User>();
                cfg.CreateMap<User, ProfileDto>();
            }).CreateMapper();

        private string GetHashedPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to generate JWT token", ex);
            }
        }

        private string GenerateResetPasswordRequestCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(Enumerable.Repeat(chars, 8).Select(s => s[new Random().Next(s.Length)]).ToArray());
            return result;
        }

        public async Task<bool> ActivateAsync(int userId)
        {
            var result = await GetAsync(userId);
            result.IsActive = true;
            _context.Users.Update(result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task ChangePasswordAsync(int userId, string password)
        {
            var result = await GetAsync(userId);
            result.Password = GetHashedPassword(password);
            result.IsActive = true;
            _context.Users.Update(result);
            await _context.SaveChangesAsync();
        }

        public async Task<User> AddUser(User user)
        {
            var existing = await GetByEmailAsync(user.Email);
            if (existing != null)
            {
                return null;
            }
            else
            {
                user.Password = GetHashedPassword(user.Password);
                user.CreatedOn = DateTime.Now;
                user.IsActive = false;
                var result = (await _context.Users.AddAsync(user)).Entity;
                await _context.SaveChangesAsync();

                return result;
            }
        }

        public async Task DeleteUser(int userId)
        {
            var user = await GetAsync(userId);
            _context.ResetPasswordRequests.RemoveRange(_context.ResetPasswordRequests.Where(x => x.UserId == userId));
            _context.Orders.RemoveRange(_context.Orders.Where(x => x.UserId == userId));
            _context.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(ProfileDto user)
        {
            var result = await GetAsync(user.Id);
            result.Email = user.Email;
            result.FirstName = user.FirstName;
            result.LastName = user.LastName;
            result.City = user.City;
            result.Country = user.Country;
            result.Ethnicity = user.Ethnicity;
            result.Facebook = user.Facebook;
            result.Gender = user.Gender;
            result.Instagram = user.Instagram;
            result.Phone = user.LastName;

            if (!string.IsNullOrEmpty(user.Password))
                result.Password = GetHashedPassword(user.Password);

            await SaveUser(result);
        }

        public async Task SaveUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var hashedPassword = GetHashedPassword(model.Password);

            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Email == model.Email &&
                x.Password == hashedPassword &&
                x.IsActive == true);

            if (user == null)
                return null;

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            return result;
        }

        public async Task<ResetPasswordRequest> GenerateResetPasswordRequest(int userId)
        {
            var result = new ResetPasswordRequest()
            {
                UserId = userId,
                RequestedOn = DateTime.Now,
                Code = GenerateResetPasswordRequestCode()
            };

            await _context.ResetPasswordRequests.AddAsync(result);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ResetPasswordRequest> GetResetPasswordRequest(string code)
        {
            return await _context.ResetPasswordRequests.FirstOrDefaultAsync(x => x.Code == code);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task SaveAllUsers(IEnumerable<User> users)
        {
            _context.Users.UpdateRange(users);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }


        public async Task<BaseResponse<JwtResponseVM>> SignInWithGoogle(GoogleSignInVM model)
        {

            var response = await _googleAuthService.GoogleSignIn(model);

            if (response.Errors.Any())
                return new BaseResponse<JwtResponseVM>(response.ResponseMessage, response.Errors);

            var jwtResponse = CreateJwtToken(response.Data);

            var data = new JwtResponseVM
            {
                Token = jwtResponse,
            };

            return new BaseResponse<JwtResponseVM>(data);
        }

        public async Task<BaseResponse<JwtResponseVM>> SignInWithFacebook(FacebookSignInVM model)
        {
            var validatedFbToken = await _facebookAuthService.ValidateFacebookToken(model.AccessToken);

            if (validatedFbToken.Errors.Any())
                return new BaseResponse<JwtResponseVM>(validatedFbToken.ResponseMessage, validatedFbToken.Errors);

            var userInfo = await _facebookAuthService.GetFacebookUserInformation(model.AccessToken);

            if (userInfo.Errors.Any())
                return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = userInfo.Data.FirstName,
                LastName = userInfo.Data.LastName,
                Email = userInfo.Data.Email,
                ProfilePicture = userInfo.Data.Picture.Data.Url.AbsoluteUri,
                LoginProviderSubject = userInfo.Data.Id,
            };

            var user = await AddUserFromSocial(_userManager, _context, userToBeCreated, LoginProvider.Facebook);

            if (user is not null)
            {
                var jwtResponse = CreateJwtToken(user);

                var data = new JwtResponseVM
                {
                    Token = jwtResponse,
                };

                return new BaseResponse<JwtResponseVM>(data);
            }

            return new BaseResponse<JwtResponseVM>(null, userInfo.Errors);

        }

        private string CreateJwtToken(User user)
        {

            var key = Encoding.ASCII.GetBytes(_jwt.Value.Secret);

            var userClaims = BuildUserClaims(user);

            var signKey = new SymmetricSecurityKey(key);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Value.ValidIssuer,
                notBefore: DateTime.UtcNow,
                audience: _jwt.Value.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_jwt.Value.DurationInMinutes)),
                claims: userClaims,
                signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private List<Claim> BuildUserClaims(User user)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.GivenName, user.FirstName),
                new Claim(JwtClaimTypes.FamilyName, user.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            return userClaims;
        }

        public static async Task<User> AddUserFromSocial(UserManager<User> userManager, DefaultContext context, CreateUserFromSocialLogin model, LoginProvider loginProvider)
        {
            //CHECKS IF THE USER HAS NOT ALREADY BEEN LINKED TO AN IDENTITY PROVIDER
            var user = await userManager.FindByLoginAsync(loginProvider.GetDisplayName(), model.LoginProviderSubject);

            if (user is not null)
                return user; //USER ALREADY EXISTS.

            user = await userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Avatar = model.ProfilePicture,
                    Password = string.Empty,
                    Gender = Gender.Male
                };

                await userManager.CreateAsync(user);

                //EMAIL IS CONFIRMED; IT IS COMING FROM AN IDENTITY PROVIDER
                user.IsActive = true;

                await userManager.UpdateAsync(user);
                await context.SaveChangesAsync();
            }

            UserLoginInfo userLoginInfo = null;
            switch (loginProvider)
            {
                case LoginProvider.Google:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                case LoginProvider.Facebook:
                    {
                        userLoginInfo = new UserLoginInfo(loginProvider.GetDisplayName(), model.LoginProviderSubject, loginProvider.GetDisplayName().ToUpper());
                    }
                    break;
                default:
                    break;
            }

            //ADDS THE USER TO AN IDENTITY PROVIDER
            var result = await userManager.AddLoginAsync(user, userLoginInfo);

            if (result.Succeeded)
                return user;

            else
                return null;
        }

    }
}