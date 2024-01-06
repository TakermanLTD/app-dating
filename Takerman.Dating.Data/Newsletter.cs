using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Takerman.Dating.Data
{
    public class Newsletter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonPropertyName("email")]
        [DataType(DataType.EmailAddress)]
        [Description("Email")]
        [StringLength(300)]
        public string Email { get; set; }

        [JsonIgnore]
        [JsonPropertyName("userId")]
        [Description("User ID")]
        public int? UserId { get; set; }

        [JsonIgnore]
        [DataType(DataType.DateTime)]
        [Description("SentOn")]
        public DateTime SubscribedOn { get; set; }
    }
}