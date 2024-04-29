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
            var result = await _datingService.GetCard(userId, id);

            result.Pictures = await _cdnService.GetDatePictures(Enum.Parse<Ethnicity>(result.Ethnicity));

            return result;
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

        [HttpPost("Add")]
        public async Task<Date> Add(Date date)
        {
            return await _datingService.Add(date);
        }

        [HttpPut("Save")]
        public async Task Save([FromBody] Date date)
        {
            await _datingService.Save(date);
        }

        [HttpPut("SaveAll")]
        public async Task SaveAll([FromBody] IEnumerable<Date> dates)
        {
            await _datingService.SaveAll(dates);
        }

        [HttpDelete("Delete")]
        public async Task Delete(int id)
        {
            await _datingService.Delete(id);
        }

        [HttpDelete("DeleteAll")]
        public async Task DeleteAll()
        {
            await _datingService.DeleteAll();
        }

        [HttpDelete("DeleteSpots")]
        public async Task DeleteSpots()
        {
            await _datingService.DeleteSpots();
        }
    }
}