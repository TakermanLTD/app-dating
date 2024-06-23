using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Services.Authentication
{
    public class FacebookSignInVM
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;
    }
}
