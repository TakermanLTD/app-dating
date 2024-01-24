using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatesController(IDatingService datingService, ILogger<DatesController> logger) : ControllerBase
    {
        private readonly IDatingService _datingService = datingService;
        private readonly ILogger<DatesController> _logger = logger;

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Date>> GetAll()
        {
            return await _datingService.GetAll();
        }

        [HttpGet("SaveSpot")]
        public async Task<IEnumerable<Date>> SaveSpot()
        {
            return await _datingService.GetAll();
        }
    }
}