namespace Takerman.Dating.Services.Authentication
{
    public class CreateUserFromSocialLogin
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ProfilePicture { get; set; } = string.Empty;
        
        public string LoginProviderSubject { get; set; } = string.Empty;
    }
}
