using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController(IAdminService _adminService, IUserService _userService, INotificationService _notificationService, ILogger<AdminController> _logger) : BaseController(_logger)
    {
        [HttpPost("AddDate")]
        public async Task<Date> AddDate(Date date)
        {
            return await _adminService.AddDate(date);
        }

        [HttpPut("SaveDate")]
        public async Task SaveDate([FromBody] Date date)
        {
            await _adminService.SaveDate(date);
        }

        [HttpPut("SaveAllDates")]
        public async Task SaveAllDates([FromBody] IEnumerable<Date> dates)
        {
            await _adminService.SaveAllDates(dates);
        }

        [HttpDelete("DeleteDate")]
        public async Task DeleteDate(int id)
        {
            await _adminService.DeleteDate(id);
        }

        [HttpDelete("DeleteAllDates")]
        public async Task DeleteAllDates()
        {
            await _adminService.DeleteAllDates();
        }

        [HttpDelete("DeleteAllSpots")]
        public async Task DeleteAllSpots()
        {
            await _adminService.DeleteAllSpots();
        }

        [HttpDelete("DeleteAllOrders")]
        public async Task DeleteAllOrders()
        {
            await _adminService.DeleteAllOrders();
        }

        [HttpDelete("ResetAllDates")]
        public async Task ResetAllDates()
        {
            await _adminService.ResetAllDates();
        }

        [HttpPost("AddUser")]
        public async Task<User> AddUser([FromBody] User user)
        {
            user.IsActive = true;
            return await _userService.AddUser(user);
        }

        //[Authorize]
        [HttpDelete("DeleteUser")]
        public async Task DeleteUser(int userId)
        {
            var user = await _userService.GetAsync(userId);

            await _userService.DeleteUser(userId);

            await _notificationService.NotifyForDeletedUser(user);
        }

        //[Authorize]
        [HttpPut("SaveUser")]
        public async Task SaveUser([FromBody] User user)
        {
            await _userService.SaveUser(user);
        }

        [HttpPut("SaveAllUsers")]
        public async Task SaveAllUsers([FromBody] IEnumerable<User> users)
        {
            await _userService.SaveAllUsers(users);
        }

        [HttpPut("DeleteAllUsers")]
        public async Task DeleteAllUsers()
        {
            await _userService.DeleteAllUsers();
        }

        [HttpPut("ResetAllUsers")]
        public async Task ResetAllUsers()
        {
            await _adminService.ResetAllUsers();
        }
    }
}