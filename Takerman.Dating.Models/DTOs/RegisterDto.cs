namespace Takerman.Dating.Data.DTOs
{
    public class RegisterDto
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string ConfirmPassword { get; set; }

        public required Gender Gender { get; set; }
    }
}