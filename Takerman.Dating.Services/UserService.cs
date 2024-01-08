using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class UserService : IUserService
    {
        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, User>();
                cfg.CreateMap<User, ProfileDto>();
            }).CreateMapper();
        }

        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public async Task<bool> ActivateAsync(int userId)
        {
            await using var context = new DefaultContext();
            var result = await GetAsync(userId);
            result.IsActive = true;
            context.Users.Update(result);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task ChangePasswordAsync(int userId, string password)
        {
            await using var context = new DefaultContext();
            var result = await GetAsync(userId);
            result.Password = GetHashedPassword(password);
            result.IsActive = true;
            context.Users.Update(result);
            await context.SaveChangesAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            await using var context = new DefaultContext();

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
                var result = (await context.Users.AddAsync(user)).Entity;
                await context.SaveChangesAsync();

                return result;
            }
        }

        public async Task DeleteAsync(int userId)
        {
            await using var context = new DefaultContext();
            var user = await GetAsync(userId);
            context.ResetPasswordRequests.RemoveRange(context.ResetPasswordRequests.Where(x => x.UserId == userId));
            context.Orders.RemoveRange(context.Orders.Where(x => x.UserId == userId));
            context.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetAsync(int id)
        {
            await using var context = new DefaultContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(ProfileDto user)
        {
            await using var context = new DefaultContext();
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

            context.Users.Update(result);
            await context.SaveChangesAsync();
        }

        private string GetHashedPassword(string password)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
                builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }

        public async Task<ProfileDto> Authenticate(AuthenticateRequest model)
        {
            await using var context = new DefaultContext();
            var hashedPassword = GetHashedPassword(model.Password);

            var user = await context.Users.FirstOrDefaultAsync(x =>
                x.Email == model.Email &&
                x.Password == hashedPassword &&
                x.IsActive == true);

            if (user == null)
                return null;

            var token = GenerateJwtToken(user);

            var result = _mapper.Map<ProfileDto>(user);
            result.Token = token;

            return result;
        }

        private string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
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

        public async Task<User> GetByEmailAsync(string email)
        {
            await using var context = new DefaultContext();
            return await context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<ResetPasswordRequest> GenerateResetPasswordRequest(int userId)
        {
            await using var context = new DefaultContext();

            var result = new ResetPasswordRequest()
            {
                UserId = userId,
                RequestedOn = DateTime.Now,
                Code = GenerateResetPasswordRequestCode()
            };

            await context.ResetPasswordRequests.AddAsync(result);

            await context.SaveChangesAsync();

            return result;
        }

        private string GenerateResetPasswordRequestCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new string(Enumerable.Repeat(chars, 8).Select(s => s[new Random().Next(s.Length)]).ToArray());
            return result;
        }

        public async Task<ResetPasswordRequest> GetResetPasswordRequest(string code)
        {
            await using var context = new DefaultContext();

            return await context.ResetPasswordRequests.FirstOrDefaultAsync(x => x.Code == code);
        }
    }
}