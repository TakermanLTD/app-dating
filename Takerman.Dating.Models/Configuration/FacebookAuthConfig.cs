namespace Takerman.Dating.Models.Configuration
{
    public class FacebookAuthConfig
    {
        public string TokenValidationUrl { get; set; } = string.Empty;
        
        public string UserInfoUrl { get; set; } = string.Empty;

        public string AppId { get; set; } = string.Empty;

        public string AppSecret { get; set; }  = string.Empty;
    }
}
