using Microsoft.AspNetCore.Identity;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Models.Enum;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IUserService
    {
        Task<bool> ActivateAsync(int userId);

        Task ChangePasswordAsync(int userId, string password);

        Task<User> AddUser(User user);

        Task DeleteUser(int userId);

        Task<User> GetAsync(int id);

        Task UpdateAsync(ProfileDto user);

        Task SaveUser(User user);

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        Task<User> GetByEmailAsync(string email);

        Task<ResetPasswordRequest> GenerateResetPasswordRequest(int userId);

        Task<ResetPasswordRequest> GetResetPasswordRequest(string code);

        Task<IEnumerable<User>> GetAllAsync();

        Task SaveAllUsers(IEnumerable<User> users);

        Task DeleteAllUsers();
    }
}