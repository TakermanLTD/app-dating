using Microsoft.AspNetCore.Identity;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Models.Enum;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IAuthService
    {
        Task<User> AddUserFromSocial(UserManager<User> userManager, DefaultContext context, CreateUserFromSocialLogin model, LoginProvider loginProvider);
    }
}