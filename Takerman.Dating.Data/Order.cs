using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [DataType(DataType.Text)]
        public string Currency { get; set; }

        public OrderStatus Status { get; set; }

        public string PaymentId { get; set; }

        public string PayerId { get; set; }

        public string PaymentSource { get; set; }

        public string OrderId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Date))]
        public int DateId { get; set; }
    }
}