using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class AdminService(DefaultContext _context, ILogger<DatingService> _logger) : IAdminService
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DateCardDto, Date>();
            cfg.CreateMap<Date, DateCardDto>();
        }).CreateMapper();

        public async Task SaveDate(Date date)
        {
            date.Orders = await _context.Orders.Where(x => x.DateId == date.Id).ToListAsync();
            // date.Attendees = await _context.UserSavedSpots.Where(x => x.DateId == date.Id).Select(x => x.User).ToListAsync();
            _context.Dates.Update(date);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDate(int id)
        {
            var date = await _context.Dates.FirstAsync(x => x.Id == id);
            _context.Dates.Remove(date);
            await _context.SaveChangesAsync();
        }

        public async Task<Date> AddDate(Date date)
        {
            await _context.Dates.AddAsync(date);
            await _context.SaveChangesAsync();
            return date;
        }

        public async Task SaveAllDates(IEnumerable<Date> dates)
        {
            _context.Dates.UpdateRange(dates);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllDates()
        {
            _context.Dates.RemoveRange(_context.Dates);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAllSpots()
        {
            _context.UserSavedSpots.RemoveRange(_context.UserSavedSpots);
            foreach (var date in _context.Dates)
            {
                date.MenCount = 0;
                date.WomenCount = 0;
                date.Status = DateStatus.NotApproved;
            }
            _context.Dates.UpdateRange(_context.Dates);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAllOrders()
        {
            _context.ChatMessages.RemoveRange(_context.ChatMessages);
            _context.DateUserChoices.RemoveRange(_context.DateUserChoices);
            _context.Orders.RemoveRange(_context.Orders);
            return _context.SaveChangesAsync();
        }

        public Task ResetAllDates()
        {
            foreach (var date in _context.Dates)
            {
                date.StartsOn = null;
                date.MenCount = 0;
                date.WomenCount = 0;
                date.Status = DateStatus.NotApproved;
                date.Orders = [];
            }
            return _context.SaveChangesAsync();
        }

        public Task ResetAllUsers()
        {
            foreach (var user in _context.Users)
            {
                user.IsActive = true;
                user.Choices = [];
            }
            return _context.SaveChangesAsync();
            
        }
    }
}