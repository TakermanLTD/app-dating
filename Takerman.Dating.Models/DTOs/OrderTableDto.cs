using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Models.DTOs
{
    public class OrderTableDto
    {
        [Description("Id")]
        public int Id { get; set; }

        [DataType(DataType.Currency)]
        [Description("Total")]
        public decimal Total { get; set; }

        [DataType(DataType.Text)]
        [Description("Currency")]
        public string Currency { get; set; }

        [Description("Quantity")]
        public int Quantity { get; set; }

        [Description("Color")]
        public string Color { get; set; }

        [Description("Status")]
        public string Status { get; set; }

        [Description("Upload")]
        public string Upload { get; set; }

        [Description("Refundable")]
        public bool Refundable { get; set; }

        [DataType(DataType.Text)]
        [Description("Tracking Code")]
        public string TrackingCode { get; set; }
    }
}