using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatesController(IDatingService _datingService, ICdnService _cdnService, ILogger<DatesController> _logger) : ControllerBase
    {
        [HttpGet("Get")]
        public async Task<DateCardDto> Get(int id, int? userId)
        {
            var date = await _datingService.Get(id);
            var result = await _datingService.GetCardFromDate(date);

            var thumbnail = await _cdnService.GetDateThumbnail();

            result.Pictures = [thumbnail];

            return result;
        }

        [HttpGet("GetDate")]
        public async Task<DateCardDto> GetDate(int id, int? userId)
        {
            await _datingService.UpdateStatus(id);
            var date = await _datingService.Get(id);
            var result = await _datingService.GetCardFromDate(date);

            result.Pictures = await _cdnService.GetDatePictures();

            return result;
        }

        [HttpGet("IsBought")]
        public async Task<bool> IsBought(int userId, int dateId)
        {
            return await _datingService.IsBought(userId, dateId);
        }

        [HttpGet("IsSpotSaved")]
        public async Task<bool> IsSpotSaved(int userId, int dateId)
        {
            return await _datingService.IsSpotSaved(userId, dateId);
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
        public async Task<IEnumerable<DateCardDto>> GetSavedSpots(int userId)
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