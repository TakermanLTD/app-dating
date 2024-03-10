using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class DatePicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Date))]
        public int DateId { get; set; }

        public virtual Date? Date { get; set; } = null;

        [MaxLength(int.MaxValue)]
        public string Data { get; set; }

        public DateTime UploadedOn { get; set; } = DateTime.Now;
    }
}