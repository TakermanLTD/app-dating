using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class ResetPasswordRequest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Description("Code")]
        [StringLength(200)]
        public string Code { get; set; }

        [Description("User ID")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [DataType(DataType.DateTime)]
        [Description("RequestedOn")]
        public DateTime RequestedOn { get; set; }

        [Description("User")]
        public virtual User User { get; set; } = null;
    }
}