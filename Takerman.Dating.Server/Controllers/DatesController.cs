using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatesController(IDatingService datingService, ILogger<DatesController> logger) : ControllerBase
    {
        private readonly IDatingService _datingService = datingService;
        private readonly ILogger<DatesController> _logger = logger;

        [HttpPost("GetAll")]
        public async Task<IEnumerable<DateCardDto>> GetAll(int? userId, FilterDto filter)
        {
            return await _datingService.GetAllAsCards(userId, filter);
        }

        [HttpGet("SaveSpot")]
        public async Task<DateCardDto> SaveSpot(int userId, int dateId)
        {
            return await _datingService.SaveSpot(userId, dateId);
        }

        [HttpGet("UnsaveSpot")]
        public async Task<DateCardDto> UnsaveSpot(int userId, int dateId)
        {
            return await _datingService.UnsaveSpot(userId, dateId);
        }
    }
}