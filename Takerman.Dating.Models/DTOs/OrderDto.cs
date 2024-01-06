using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Takerman.Dating.Models.DTOs
{
    public class OrderDto
    {
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

        [Description("Number")]
        public int Number { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Description("Phone")]
        [StringLength(100)]
        public string Phone { get; set; }

        [Description("Quantity")]
        public short Quantity { get; set; }

        [Description("Payment Provider")]
        public string PaymentProvider { get; set; }

        [Description("User Id")]
        public int UserId { get; set; }

        [Description("Color Id")]
        public int ColorId { get; set; }

        [Description("Upload Id")]
        public int UploadId { get; set; }

        [Description("Tracking Code")]
        [DataType(DataType.Text)]
        public string TrackingCode { get; set; }
    }
}