using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;
using Takerman.Dating.Services.Authentication;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService _userService, INotificationService _notificationService) : ControllerBase
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

            var result = await _userService.CreateAsync(user);

            await _notificationService.NotifyForCreatedUser(result, HttpContext.Request.Host.Value);

            return result;
        }

        [HttpPost("AdminAdd")]
        public async Task<User> AdminAdd([FromBody] User user)
        {
            return await _userService.CreateAsync(user);
        }

        //[Authorize]
        [HttpDelete("Delete")]
        public async Task Delete(int userId)
        {
            var user = await _userService.GetAsync(userId);

            await _userService.DeleteAsync(userId);

            await _notificationService.NotifyForDeletedUser(user);
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

        //[Authorize]
        [HttpPut("AdminSave")]
        public async Task AdminSave([FromBody] User user)
        {
            await _userService.UpdateAsync(user);
        }

        [HttpPut("SaveAll")]
        public async Task SaveAll([FromBody] IEnumerable<User> users)
        {
            await _userService.SaveAll(users);
        }

        [HttpPut("DeleteAll")]
        public async Task DeleteAll()
        {
            await _userService.DeleteAll();
        }
    }
}