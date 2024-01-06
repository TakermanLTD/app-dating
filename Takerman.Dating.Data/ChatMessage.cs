using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class ChatMessage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ToUserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }

        public DateTime SentOn { get; set; }
    }
}