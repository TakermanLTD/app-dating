using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IOrderService
    {
        Task<Order> GetAsync(int id);

        Task<Order> CreateAsync(Order order);

        Task<Order> CreateAsync(OrderDto order);

        Task<Order> UpdateAsync(Order order);

        Task ChangeStatusAsync(Order order, OrderStatus orderStatus);

        Task<IEnumerable<Order>> GetByUserIdAsync(int userId);
        
        Task<Order> CancelAsync(int id);
    }
}