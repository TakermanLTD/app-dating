using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IAdminService
    {
        Task<Date> AddDate(Date date);

        Task SaveDate(Date date);

        Task DeleteDate(int id);

        Task SaveAllDates(IEnumerable<Date> dates);

        Task DeleteAllDates();

        Task DeleteAllSpots();
        
        Task DeleteAllOrders();

        Task ResetAllDates();
        
        Task ResetAllUsers();
    }
}