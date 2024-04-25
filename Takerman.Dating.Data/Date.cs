using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Takerman.Dating.Data
{
    public class Date
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public short MinMen { get; set; }

        public short MinWomen { get; set; }

        public short MenCount { get; set; }

        public short WomenCount { get; set; }

        public DateStatus Status { get; set; }

        public string? VideoLink { get; set; }

        public DateType DateType { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(500)]
        public string ShortDescription { get; set; }

        public DateTime? StartsOn { get; set; }

        public short MinAges { get; set; }

        public short MaxAges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? Location { get; set; }

        public Ethnicity Ethnicity { get; set; }

        // public virtual ICollection<User> Attendees { get; set; } = null;

        public virtual ICollection<Order> Orders { get; set; } = null;

    }
}