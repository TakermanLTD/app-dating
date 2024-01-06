using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

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

        [Description("User ID")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Description("User")]
        public virtual User User { get; set; } = null;
    }
}