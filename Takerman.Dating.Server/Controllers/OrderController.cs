using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController(IOrderService _orderService, INotificationService _notificationService, IDatingService _datingService, ILogger _logger) : BaseController(_logger)
    {
        [HttpGet("Get")]
        public async Task<Order> Get(int id)
        {
            return await _orderService.GetAsync(id);
        }

        [HttpGet("GetByUserId")]
        public async Task<IEnumerable<OrderTableDto>> GetByUserId(int userId)
        {
            var orders = await _orderService.GetByUserIdAsync(userId);

            var result = orders.Select(x => new OrderTableDto()
            {
                Id = x.Id,
                Currency = x.Currency,
                Total = x.Total,
                Status = Enum.GetName(x.Status) ?? string.Empty,
                Refundable = x.CreatedOn.AddDays(14) > DateTime.Now,
                Date = _datingService.GetCardFromDate(userId, x.DateId).Result
            }).ToList();

            return result;
        }

        [HttpPost("Create")]
        public async Task<Order> Create(OrderDto orderDto)
        {
            var result = await _orderService.CreateAsync(orderDto);
            await _notificationService.NotifyForOrderCreatedAsync(result);
            return result;
        }

        [HttpPut("Cancel")]
        public async Task<Order> Cancel(int id)
        {
            var result = await _orderService.CancelAsync(id);
            await _notificationService.NotifyForOrderCanceledAsync(result);
            return result;
        }
    }
}