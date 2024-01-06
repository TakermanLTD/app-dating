namespace Takerman.Dating.Services.Authentication;

using System.ComponentModel.DataAnnotations;

public class ResetPasswordResponseModel
{
    public int UserId { get; set; }

    [Required]
    public string Password { get; set; }
}