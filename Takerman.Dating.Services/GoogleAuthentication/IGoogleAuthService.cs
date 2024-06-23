using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Services.GoogleAuthentication
{
    public interface IGoogleAuthService
    {
        Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model);
    }
}
