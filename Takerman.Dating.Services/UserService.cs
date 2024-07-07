using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Services
{
    public class UserService(IOptions<AppSettings> _appSettings, DefaultContext _context, ILogger<UserService> _logger) : IUserService
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
                return existing;
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to authenticate the user");
                throw;
            }
        }

        public async Task<AuthenticateResponse> Authenticate(User user)
        {
            try
            {
                var token = GenerateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to authenticate the user");
                throw;
            }
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

        public Task<User> GetByFacebookId(string facebookId)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Facebook == facebookId);
        }

        public async Task UpdateFacebookIdAsync(string email, string facebookId)
        {
            var user = await GetByEmailAsync(email);
            user.Facebook = facebookId;
            await _context.SaveChangesAsync();
        }
    }
}