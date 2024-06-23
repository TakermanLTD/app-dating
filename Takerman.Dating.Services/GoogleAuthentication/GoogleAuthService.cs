using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations;
using Takerman.Dating.Data;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Models.Enum;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Takerman.Dating.Services.GoogleAuthentication
{
    public class GoogleAuthService(UserManager<User> userManager, DefaultContext context, IOptions<GoogleAuthConfig> googleAuthConfig, ILogger<GoogleAuthService> _logger, IUserService _userService) : IGoogleAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly DefaultContext _context = context;
        private readonly GoogleAuthConfig _googleAuthConfig = googleAuthConfig.Value;

        public async Task<BaseResponse<User>> GoogleSignIn(GoogleSignInVM model)
        {

            Payload payload = new();

            try
            {
                payload = await ValidateAsync(model.IdToken, new ValidationSettings
                {
                    Audience = new[] { _googleAuthConfig.ClientId }
                });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return new BaseResponse<User>(null, new List<string> { "Failed to get a response" });
            }

            var userToBeCreated = new CreateUserFromSocialLogin
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                ProfilePicture = payload.Picture,
                LoginProviderSubject = payload.Subject,
            };
            
           var user = await _userService.AddUserFromSocial(_userManager, _context, userToBeCreated, LoginProvider.Google);

            if (user is not null)
                return new BaseResponse<User>(user);

            else
                return new BaseResponse<User>(null, new List<string> { "Unable to link a Local User to a Provider" });
        }

        
    }
}
