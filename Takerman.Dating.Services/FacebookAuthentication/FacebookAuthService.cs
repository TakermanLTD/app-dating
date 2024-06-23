using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Models.DTOs;
using Microsoft.Extensions.Http;

namespace Takerman.Dating.Services.FacebookAuthentication
{
    public class FacebookAuthService(IHttpClientFactory httpClientFactory, IOptions<FacebookAuthConfig> facebookAuthConfig, ILogger<FacebookAuthService> _logger) : IFacebookAuthService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient("Facebook");
        private readonly FacebookAuthConfig _facebookAuthConfig = facebookAuthConfig.Value;

        public async Task<BaseResponse<FacebookTokenValidationResponse>> ValidateFacebookToken(string accessToken)
        {
            try
            {
                string TokenValidationUrl = _facebookAuthConfig.TokenValidationUrl;
                var url = string.Format(TokenValidationUrl, accessToken, _facebookAuthConfig.AppId, _facebookAuthConfig.AppSecret);
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    var tokenValidationResponse = JsonConvert.DeserializeObject<FacebookTokenValidationResponse>(responseAsString);
                    return new BaseResponse<FacebookTokenValidationResponse>(tokenValidationResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, ex);
            }

            return new BaseResponse<FacebookTokenValidationResponse>(null, "Failed to get response");

        }

        public async Task<BaseResponse<FacebookUserInfoResponse>> GetFacebookUserInformation(string accessToken)
        {
            try
            {
                string userInfoUrl = _facebookAuthConfig.UserInfoUrl;
                string url = string.Format(userInfoUrl, accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var userInfoResponse = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);
                    return new BaseResponse<FacebookUserInfoResponse>(userInfoResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace, ex);
            }

            return new BaseResponse<FacebookUserInfoResponse>(null, "Failed to get response");

        }
    }
}
