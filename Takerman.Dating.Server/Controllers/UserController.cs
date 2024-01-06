using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, User>();
                cfg.CreateMap<User, RegisterDto>();
            }).CreateMapper();
        }

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

        [HttpPost("Create")]
        public async Task<User> Create([FromBody] RegisterDto data)
        {
            var user = _mapper.Map<User>(data);
            user.IsActive = true;

            var result = await _userService.CreateAsync(user);

            await _notificationService.NotifyForCreatedUser(result, HttpContext.Request.Host.Value);

            return result;
        }

        [HttpDelete("Delete")]
        public async Task Delete(int userId)
        {
            var user = await _userService.GetAsync(userId);

            await _userService.DeleteAsync(userId);

            await _notificationService.NotifyForDeletedUser(user);
        }

        [HttpPut("Update")]
        public async Task Update([FromBody] RegisterDto data)
        {
            var user = _mapper.Map<User>(data);
            user.IsActive = true;

            if (!string.IsNullOrEmpty(data.Password) && data.Password.ToLower() == data.ConfirmPassword.ToLower())
            {
                user.Password = data.Password;
            }

            await _userService.UpdateAsync(user);
        }
    }
}