using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UploadController(
        IUploadService uploadService,
        INotificationService notificationService,
        IUserService userService,
        ISpecificationService specificationService,
        IOrderService orderService) : ControllerBase
    {
        private readonly IUploadService _uploadService = uploadService;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IUserService _userService = userService;
        private readonly ISpecificationService _specificationService = specificationService;
        private readonly IOrderService _orderService = orderService;

        [HttpGet("Get")]
        public async Task<Upload> Get(int id)
        {
            return await _uploadService.GetAsync(id);
        }

        [HttpGet("GetByUserId")]
        public async Task<IEnumerable<Upload>> GetUploadByUserId(int userId)
        {
            return await _uploadService.GetByUserId(userId);
        }

        [HttpDelete("Delete")]
        public async Task Delete(int id)
        {
            await _uploadService.DeleteAsync(id);
        }

        [HttpGet("Download")]
        public async Task<IActionResult> Download(int id, int userId)
        {
            var user = await _userService.GetAsync(userId);
            var upload = await _uploadService.DownloadAsync(id);

            if ((userId != upload.UserId || user == null) && user.Email.ToLower() != "tivanov@takerman.net")
                throw new UnauthorizedAccessException("You are not authorized to download this file. You have to login to your profile.");

            return File(new MemoryStream(upload.UploadData.Data), "application/octet-stream", upload.Name);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(int userId, IFormFile file)
        {
            var model = new Upload();

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            model.UploadData = new UploadData
            {
                Data = ms.ToArray(),
            };
            model.SizeKB = (int)(file.Length / 1000);
            model.Name = file.FileName;
            model.Type = file.FileName.Contains('.') ? file.FileName[file.FileName.LastIndexOf('.')..] : file.FileName;
            model.UserId = userId;
            model.UploadedOn = DateTime.Now;

            await _uploadService.CreateAsync(model);

            await _notificationService.NotifyForUploadAsync(userId, model);

            return Ok(model);
        }

        [HttpGet("GetColors")]
        public async Task<List<Color>> GetColors()
        {
            return (await _specificationService.GetAvailableColorsAsync()).ToList();
        }
    }
}