using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Takerman.Dating.Data
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        [DataType(DataType.MultilineText)]
        [Description("Text")]
        [StringLength(400)]
        public string Text { get; set; }

        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress)]
        [Description("Email")]
        [StringLength(300)]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        [DataType(DataType.Text)]
        [Description("Name")]
        [StringLength(300)]
        public string Name { get; set; }

        [DataType(DataType.DateTime)]
        [Description("SentOn")]
        public DateTime SentOn { get; set; }
    }
}