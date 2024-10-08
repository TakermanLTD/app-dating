using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Server.Controllers
{
    public class FacebookTokenValidationRequest
    {
        public string AccessToken { get; set; } = string.Empty;
    }

    public class FacebookTokenValidationResponse
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Hometown { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService _userService, INotificationService _notificationService, ILogger<UserController> _logger) : ControllerBase
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, User>();
                cfg.CreateMap<User, RegisterDto>();
            }).CreateMapper();

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("ResetPasswordRequest")]
        public async Task<User> ResetPasswordRequest(ResetPasswordModel model)
        {
            var user = await _userService.GetByEmailAsync(model.Email);

            if (user == null)
                return null;

            var response = await _userService.GenerateResetPasswordRequest(user.Id);

            await _notificationService.NotifyForResetPasswordRequest(response.Code, user, HttpContext.Request.Host.Value);

            return user;
        }

        [HttpPost("ChangePassword")]
        public async Task ChangePassword(ResetPasswordResponseModel model)
        {
            await _userService.ChangePasswordAsync(model.UserId, model.Password);
        }

        [HttpGet("ResetPassword")]
        public async Task<ResetPasswordRequest> ResetPassword(string id)
        {
            return await _userService.GetResetPasswordRequest(id);
        }

        [HttpGet("Get")]
        public async Task<User> Get(int id)
        {
            return await _userService.GetAsync(id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userService.GetAllAsync();
        }

        [HttpPut("ChangePassword")]
        public async Task ChangePassword(int id, string password)
        {
            await _userService.ChangePasswordAsync(id, password);
        }

        [HttpGet("Activate")]
        public async Task<bool> Activate(int code)
        {
            return await _userService.ActivateAsync(code);
        }

        [HttpPost("Add")]
        public async Task<User> Add([FromBody] RegisterDto data)
        {
            var user = _mapper.Map<User>(data);
            user.IsActive = true;

            var result = await _userService.AddUser(user);

            await _notificationService.NotifyForCreatedUser(result, HttpContext.Request.Host.Value);

            return result;
        }

        //[Authorize]
        [HttpPut("Save")]
        public async Task Save([FromBody] ProfileDto user)
        {
            if (!string.IsNullOrEmpty(user.Password) && user.Password.Equals(user.ConfirmPassword, StringComparison.CurrentCultureIgnoreCase))
            {
                user.Password = user.Password;
            }

            await _userService.UpdateAsync(user);
        }

        [HttpPost("validate-facebook-token")]
        public async Task<IActionResult> ValidateFacebookToken([FromBody] FacebookTokenValidationRequest request)
        {
            var client = new HttpClient();
            var response = await client.GetFromJsonAsync<FacebookTokenValidationResponse>($"https://graph.facebook.com/me?access_token={request.AccessToken}&fields=id,name,email,gender,hometown,link,photos");

            if (response == null || response.Id == null)
            {
                return Unauthorized();
            }

            var userByFacebook = await _userService.GetByFacebookId(response.Id);
            var userByEmail = await _userService.GetByEmailAsync(response.Email);

            if (userByFacebook != null)
            {
                return Ok(await _userService.Authenticate(userByFacebook));
            }
            else if (userByEmail != null)
            {
                await _userService.UpdateFacebookIdAsync(response.Email, response.Id);
                return Ok(await _userService.Authenticate(userByEmail));
            }
            else
            {
                var firstName = response.Name[..response.Name.IndexOf(' ')];
                var lastName = response.Name[(response.Name.IndexOf(' ') + 1)..];
                var newUser = new User
                {
                    FacebookId = response.Id,
                    FacebookLink = response.Link,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = response.Email,
                    Password = string.Empty,
                    IsActive = true,
                    Gender = string.IsNullOrEmpty(response.Gender) ? Gender.Male : Enum.Parse<Gender>(response.Gender, true),
                    City = response.Hometown,
                    Ethnicity = Ethnicity.All
                };

                var addedUser = await _userService.AddUser(newUser);

                return Ok(await _userService.Authenticate(addedUser));
            }

            // var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // if (!result.Succeeded)
            //     return BadRequest("Facebook authentication failed.");
            // var claims = result.Principal.Identities.FirstOrDefault().Claims;
            // var jwtToken = GenerateJwtToken(claims);
            // return Ok(new { Token = jwtToken });
            // return Ok(response); // For demonstration purposes
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_SECRET_KEY"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "sreshti.net",
                audience: "sreshti.net",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}