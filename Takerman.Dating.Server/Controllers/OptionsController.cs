using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OptionsController(IOptionsService _optionsService, ILogger<OptionsController> _logger) : ControllerBase
    {
        [HttpGet("GetEthnicities")]
        public IEnumerable<KeyValuePair<int, string>> GetEthnicities()
        {
            return _optionsService.GetEthnicities();
        }

        [HttpGet("GetDateTypes")]
        public IEnumerable<KeyValuePair<int, string>> GetDateTypes()
        {
            return _optionsService.GetDateTypes();
        }

        [HttpGet("GetUserPictures")]
        public Task<IEnumerable<UserPicture>> GetUserPictures(int id)
        {
            return _optionsService.GetUserPictures(id);
        }

        [HttpDelete("DeletePicture")]
        public Task DeletePicture(int id)
        {
            return _optionsService.DeletePicture(id);
        }

        [HttpGet("GetAvatar")]
        public Task<string> GetAvatar(int userId) 
        {
            return _optionsService.GetAvatar(userId);
        }

        [HttpPut("UnsetAvatar")]
        public Task UnsetAvatar(int userId)
        {
            return _optionsService.UnsetAvatar(userId);
        }

        [HttpPut("SetAvatar")]
        public Task SetAvatar(int userId, int id)
        {
            return _optionsService.SetAvatar(userId, id);
        }

        [HttpPost("UploadUserPictures")]
        public Task<IEnumerable<UserPicture>> UploadUserPictures(IEnumerable<UserPicture> pictures)
        {
            return _optionsService.UploadUserPictures(pictures);
        }
    }
}