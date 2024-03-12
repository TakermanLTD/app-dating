using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CdnController(ICdnService _cdnService) : ControllerBase
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

        [HttpPut("UnsetAvatar")]
        public Task UnsetAvatar(int userId)
        {
            return _cdnService.UnsetAvatar(userId);
        }

        [HttpPut("SetAvatar")]
        public Task SetAvatar(int userId, string url)
        {
            return _cdnService.SetAvatar(userId, url);
        }

        [HttpPost("UploadUserPictures")]
        public Task<IEnumerable<UserPicture>> UploadUserPictures(UploadUserPicturesDto pictures)
        {
            return _cdnService.UploadUserPictures(pictures);
        }
    }
}