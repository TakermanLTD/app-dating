using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IUserService
    {
        Task<bool> ActivateAsync(int userId);

        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        Task ChangePasswordAsync(int userId, string password);

        Task<User> CreateAsync(User user);

        Task DeleteAsync(int userId);

        Task<User> GetAsync(int id);

        Task<User> GetByEmailAsync(string email);

        Task<ResetPasswordRequest> GetResetPasswordRequest(string id);

        Task<ResetPasswordRequest> GenerateResetPasswordRequest(int userId);

        Task UpdateAsync(ProfileDto user);

        Task<IEnumerable<User>> GetAllAsync();

        Task UpdateAsync(User user);
    }
}