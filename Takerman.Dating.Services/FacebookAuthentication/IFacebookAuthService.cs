using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.FacebookAuthentication
{
    public interface IFacebookAuthService
    {
        Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken);
        
        Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken);
    } 
}
