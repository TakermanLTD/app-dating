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

        [DataType(DataType.EmailAddress)]
        [StringLength(300)]
        public string Email { get; set; }

        [JsonIgnore]
        public int? UserId { get; set; }

        [JsonIgnore]
        [DataType(DataType.DateTime)]
        public DateTime SubscribedOn { get; set; }
    }
}