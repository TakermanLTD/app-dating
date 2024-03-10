using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class UserPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public virtual User? User { get; set; } = null;

        [MaxLength(int.MaxValue)]
        public string Data { get; set; }

        public DateTime UploadedOn { get; set; } = DateTime.Now;
    }
}