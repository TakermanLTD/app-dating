using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class OrderService(DefaultContext _context) : IOrderService
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDto, Order>();
                cfg.CreateMap<Order, OrderDto>();
            }).CreateMapper();

        public async Task ChangeStatusAsync(Order order, OrderStatus orderStatus)
        {
            var result = await GetAsync(order.Id);
            result.Status = orderStatus;
            await _context.SaveChangesAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            var result = (await _context.Orders.AddAsync(order)).Entity;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Order> CreateAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var date = await _context.Dates.FirstAsync(x => x.Id == orderDto.DateId);
            order.Status = OrderStatus.Started;
            order.CreatedOn = DateTime.Now;
            order.Currency = "EUR";
            if (date != null)
                order.Total = date.Price;
            return await CreateAsync(order);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.Orders.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            return await _context.Orders.FirstAsync(x => x.Id == id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            var result = await GetAsync(order.Id);
            result = _mapper.Map<Order>(result);
            _context.Orders.Update(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<Order> CancelAsync(int id)
        {
            var result = await GetAsync(id);
            result.Status = OrderStatus.Canceled;
            _context.Orders.Update(result);
            await _context.SaveChangesAsync();
            return result;
        }
    }
}