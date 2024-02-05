using System.ComponentModel;

namespace Takerman.Dating.Models.DTOs
{
    public class OrderDto
    {
        [Description("Payment Id")]
        public string PaymentId { get; set; }

        [Description("Payer Id")]
        public string PayerId { get; set; }

        [Description("Payment Source")]
        public string PaymentSource { get; set; }

        [Description("Order Id")]
        public string OrderId { get; set; }

        [Description("User Id")]
        public int UserId { get; set; }

        [Description("Date Id")]
        public int DateId { get; set; }
    }
}