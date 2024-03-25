using Takerman.Dating.Data;

namespace Takerman.Dating.Services.Authentication;

public class AuthenticateResponse(User user, string token)
{
    public int Id { get; set; } = user.Id;
    public string FirstName { get; set; } = user.FirstName;
    public string LastName { get; set; } = user.LastName;
    public string Email { get; set; } = user.Email;
    public string Token { get; set; } = token;
    public bool IsAdmin { get; set; } = user.IsAdmin;
}