using Takerman.Dating.Data;

namespace Takerman.Dating.Models.DTOs
{
    public class ProfileDto
    {
        public required int Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? Phone { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        public string? Token { get; set; }

        public required Gender Gender { get; set; }

        public Ethnicity? Ethnicity { get; set; }
    }
}