using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatesController(IDatingService _datingService, ILogger<DatesController> _logger) : ControllerBase
    {
        [HttpGet("Get")]
        public async Task<DateCardDto> Get(int id, int? userId)
        {
            return await _datingService.GetCard(userId, id);
        }

        [HttpGet("GetAll")]
        public async Task<IEnumerable<Date>> GetAll()
        {
            return await _datingService.GetAll();
        }

        [HttpPost("GetAllAsCards")]
        public async Task<IEnumerable<DateCardDto>> GetAllAsCards(int? userId, FilterDto filter)
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

        [HttpGet("GetMatches")]
        public async Task<IEnumerable<MatchDto>> GetMatches(int userId)
        {
            return await _datingService.GetMatches(userId);
        }

        [HttpPost("SaveChoices")]
        public async Task SaveChoices(int userId, int dateId, IEnumerable<DateChoiceDto> choices)
        {
            await _datingService.SaveChoices(userId, dateId, choices);
        }

        [HttpPost("SetStatus")]
        public async Task<DateCardDto> SetStatus(DateStatusDto status)
        {
            await _datingService.SetStatus(status);

            return await _datingService.GetCard(status.Id);
        }
    }
}