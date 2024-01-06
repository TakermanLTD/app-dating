using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;set; }

        [DataType(DataType.Text)]
        [Description("First Name")]
        [StringLength(300)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Description("Last Name")]
        [StringLength(300)]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Description("Email")]
        [StringLength(300)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Description("Password")]
        [StringLength(300)]
        public string Password { get; set; }

        [DataType(DataType.DateTime)]
        [Description("Created On")]
        public DateTime CreatedOn { get; set; }

        [Description("Orders")]
        public virtual ICollection<Order> Orders { get; set; } = null;

        [Description("Is Active")]
        public bool IsActive { get; set; }
    }
}