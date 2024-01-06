using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class Upload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Description("Downloads")]
        public int Downloads { get; set; }

        [Description("Is Removed")]
        public bool IsRemoved { get; set; }

        [Description("Size in Kilobites")]
        public int SizeKB { get; set; }

        [DataType(DataType.Text)]
        [Description("File Type")]
        [StringLength(100)]
        public string Name { get; set; }

        [Description("User ID")]
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [Description("User")]
        public virtual User User { get; set; } = null;

        [Description("Type")]
        public string Type { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UploadedOn { get; set; }

        [Description("Upload Data ID")]
        [ForeignKey(nameof(UploadData))]
        public int UploadDataId { get; set; }

        [Description("Upload Data")]
        public virtual UploadData UploadData { get; set; } = null;

        [Description("Orders")]
        public virtual IEnumerable<Order> Orders { get; set; } = null;
    }
}