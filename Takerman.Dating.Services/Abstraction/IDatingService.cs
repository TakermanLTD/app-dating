using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IDatingService
    {
        Task<Date> Get(int id);

        Task<IEnumerable<Date>> GetAll();

        Task<IEnumerable<Date>> Filter(DateFilterDto filter);

        Task SaveSpot(int userId, int dateId);

        Task Vote(int userId, int choiceId, ChoiceType choiceType);

        Task<IEnumerable<DateUserChoice>> GetDateVotesByUser(int userId, int dateId);

        Task Buy(int userId, int dateId);
    }
}