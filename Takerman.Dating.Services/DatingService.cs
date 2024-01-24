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

        public async Task<IEnumerable<DateUserChoice>> GetDateVotesByUser(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveSpot(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task Vote(int userId, int choiceId, ChoiceType choiceType)
        {
            throw new NotImplementedException();
        }
    }
}