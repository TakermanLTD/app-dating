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

        [HttpGet("GetAll")]
        public async Task<IEnumerable<DateCardDto>> GetAll(int? userId)
        {
            return await _datingService.GetAllAsCards(userId);
        }

        [HttpGet("SaveSpot")]
        public async Task SaveSpot(int userId, int dateId)
        {
            await _datingService.SaveSpot(userId, dateId);
        }

        [HttpGet("UnsaveSpot")]
        public async Task UnsaveSpot(int userId, int dateId)
        {
            await _datingService.UnsaveSpot(userId, dateId);
        }
    }
}