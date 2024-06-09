using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Takerman.Dating.Data;

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
        public string Currency { get; set; } = string.Empty;

        [Description("Status")]
        public string Status { get; set; } = string.Empty;

        [Description("Refundable")]
        public bool Refundable { get; set; }

        [Description("Date")]
        public DateCardDto Date { get; set; } = new DateCardDto();
    }
}