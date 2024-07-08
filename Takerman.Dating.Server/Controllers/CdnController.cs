using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CdnController(ICdnService _cdnService, ILogger<CdnController> _logger) : ControllerBase
    {
        [HttpGet("GetUserPictures")]
        public Task<IEnumerable<UserPicture>> GetUserPictures(int userId)
        {
            return _cdnService.GetUserPictures(userId);
        }

        [HttpDelete("DeletePicture")]
        public Task<bool> DeletePicture(int id)
        {
            return _cdnService.DeletePicture(id);
        }

        [HttpGet("GetAvatar")]
        public string GetAvatar(int userId)
        {
            return _cdnService.GetAvatar(userId);
        }

        [HttpGet("SetAvatar")]
        public Task SetAvatar(int userId, string url)
        {
            return _cdnService.SetAvatar(userId, url);
        }

        [HttpGet("SetDefaultAvatar")]
        public Task<string> SetDefaultAvatar(int userId)
        {
            return _cdnService.SetDefaultAvatar(userId);
        }

        [HttpPost("UploadUserPictures")]
        public Task<IEnumerable<UserPicture>> UploadUserPictures(int userId, IEnumerable<IFormFile> files)
        {
            return _cdnService.UploadUserPictures(userId, files);
        }
    }
}