using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
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

        [HttpGet("Get")]
        public async Task<DateCardDto> Get(int id, int? userId)
        {
            return await _datingService.GetCard(userId, id);
        }

        [HttpPost("GetAll")]
        public async Task<IEnumerable<DateCardDto>> GetAll(int? userId, FilterDto filter)
        {
            return await _datingService.GetAllAsCards(userId, filter);
        }

        [HttpGet("GetSavedSpots")]
        public async Task<IEnumerable<DateCardDto>>  GetSavedSpots(int userId)
        {
            return await _datingService.GetSavedSpots(userId);
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

        [HttpGet("GetChoices")]
        public async Task<IEnumerable<DateChoiceDto>> GetChoices(int userId, int dateId)
        {
            return await _datingService.GetChoices(userId, dateId);
        }

        [HttpPost("SaveChoices")]
        public async Task SaveChoices(int userId, int dateId, IEnumerable<DateChoiceDto> choices)
        {
            await _datingService.SaveChoices(userId, dateId, choices);
        }
    }
}