using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class EthnicPicture
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Ethnicity Ethnicity { get; set; }

        public string Url { get; set; }

        public DateTime UploadedOn { get; set; } = DateTime.Now;

        public string PublicId { get; set; } = string.Empty;
    }
}