using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;

        public OrderService()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderDto, Order>();
                cfg.CreateMap<Order, OrderDto>();
            }).CreateMapper();
        }

        public async Task ChangeStatusAsync(Order order, OrderStatus orderStatus)
        {
            await using var context = new DefaultContext();
            var result = await GetAsync(order.Id);
            result.Status = orderStatus;
            await context.SaveChangesAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await using var context = new DefaultContext();
            var result = (await context.Orders.AddAsync(order)).Entity;
            await context.SaveChangesAsync();
            return result;
        }

        public async Task<Order> CreateAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            order.Status = OrderStatus.Started;
            order.CreatedOn = DateTime.Now;
            return await CreateAsync(order);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(int userId)
        {
            await using var context = new DefaultContext();
            return await context.Orders.Include(x => x.Color).Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetAsync(int id)
        {
            await using var context = new DefaultContext();
            return await context.Orders.FirstAsync(x => x.Id == id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            await using var context = new DefaultContext();
            var result = await GetAsync(order.Id);
            result = _mapper.Map<Order>(result);
            context.Orders.Update(result);
            await context.SaveChangesAsync();
            return result;
        }

        public async Task DeleteByUploadId(int uploadId)
        {
            await using var context = new DefaultContext();
            var orders = await context.Orders.Where(x => x.UploadId == uploadId).ToListAsync();
            context.Orders.RemoveRange(orders);
            await context.SaveChangesAsync();
        }

        public async Task<Order> CancelAsync(int id)
        {
            await using var context = new DefaultContext();
            var result = await GetAsync(id);
            result.Status = OrderStatus.Canceled;
            context.Orders.Update(result);
            await context.SaveChangesAsync();
            return result;
        }
    }
}