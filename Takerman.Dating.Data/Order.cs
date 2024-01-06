using System.ComponentModel;
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
        [Description("Created On")]
        public DateTime CreatedOn { get; set; }

        [DataType(DataType.Currency)]
        [Description("Total")]
        public decimal Total { get; set; }

        [DataType(DataType.Text)]
        [Description("First Name")]
        [StringLength(300)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Description("Last Name")]
        [StringLength(300)]
        public string LastName { get; set; }

        [DataType(DataType.Text)]
        [Description("Currency")]
        public string Currency { get; set; }

        [DataType(DataType.Text)]
        [Description("Street")]
        [StringLength(300)]
        public string Street { get; set; }

        [DataType(DataType.Text)]
        [Description("City")]
        [StringLength(200)]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Description("Country")]
        [StringLength(200)]
        public string Country { get; set; }

        [Description("Street Number")]
        public int Number { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Description("Phone")]
        [StringLength(100)]
        public string Phone { get; set; }

        [Description("Order Status")]
        public OrderStatus Status { get; set; }

        [Description("Quantity")]
        public int Quantity { get; set; }

        [Description("Payment Provider")]
        public string PaymentProvider { get; set; }

        [Description("User Id")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Description("User")]
        public virtual User User { get; set; } = null;

        [Description("Color Id")]
        [ForeignKey(nameof(Color))]
        public int ColorId { get; set; }

        [Description("Color")]
        public virtual Color Color { get; set; } = null;

        [Description("Upload Id")]
        public int UploadId { get; set; }

        [Description("Tracking Code")]
        [DataType(DataType.Text)]
        public string TrackingCode { get; set; }
    }
}