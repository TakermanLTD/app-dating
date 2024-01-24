using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class DatingService(DefaultContext context, ILogger<DatingService> logger) : IDatingService
    {
        private readonly DefaultContext _context = context;
        private readonly ILogger<DatingService> _logger = logger;

        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DateCardDto, Date>();
            cfg.CreateMap<Date, DateCardDto>();
        }).CreateMapper();

        public async Task Buy(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Date>> Filter(DateFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Date> Get(int id)
        {
            return await _context.Dates.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Date>> GetAll()
        {
            return await _context.Dates.ToListAsync();
        }

        public async Task<IEnumerable<DateCardDto>> GetAllAsCards(int? userId)
        {
            var savedSpots = userId.HasValue ? await GetSavedSpots(userId.Value) : null;

            var result = new List<DateCardDto>();
            foreach (var date in _context.Dates)
            {
                var card = _mapper.Map(date, new DateCardDto());
                card.Ethnicity = date.Ethnicity.GetDisplay();
                card.IsSpotSaved = savedSpots != null && savedSpots.Any(x => x.UserId == userId.Value && x.DateId == x.DateId);
                card.DateType = date.DateType.GetDisplay();
                card.StartsOn = date.StartsOn.HasValue ? date.StartsOn.Value.ToShortDateString() : string.Empty;
                result.Add(card);
            }
            return result;
        }

        public async Task<IEnumerable<UserSavedSpot>> GetSavedSpots(int userId)
        {
            return await _context.UserSavedSpots.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<DateUserChoice>> GetDateVotesByUser(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveSpot(int userId, int dateId)
        {
            await UnsaveSpot(userId, dateId);

            await _context.UserSavedSpots.AddAsync(new UserSavedSpot
            {
                UserId = userId,
                DateId = dateId,
                SavedOn = DateTime.Now
            });

            await _context.SaveChangesAsync();
        }

        public async Task UnsaveSpot(int userId, int dateId)
        {
            var existing = _context.UserSavedSpots.Where(x => x.UserId == userId && x.DateId == dateId);
            if (existing.Any())
            {
                _context.UserSavedSpots.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Vote(int userId, int choiceId, ChoiceType choiceType)
        {
            throw new NotImplementedException();
        }
    }
}