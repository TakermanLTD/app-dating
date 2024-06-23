using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Services.Authentication
{
    public class GoogleSignInVM
    {
        [Required]
        public string IdToken { get; set; }  = string.Empty;
    }
}
