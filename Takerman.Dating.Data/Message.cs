using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400)]
        public string Text { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(300)]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        [StringLength(300)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentOn { get; set; }
    }
}