namespace Takerman.Dating.Services.Authentication;

using System.ComponentModel.DataAnnotations;

public class ResetPasswordModel
{
    [Required]
    public string Email { get; set; }
}