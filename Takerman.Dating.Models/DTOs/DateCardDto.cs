using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Takerman.Dating.Data;

namespace Takerman.Dating.Models.DTOs
{
    public class DateCardDto
    {
        public int Id { get; set; }

        public short MinMen { get; set; }

        public short MinWomen { get; set; }

        public short MenCount { get; set; }

        public short WomenCount { get; set; }

        public string? Status { get; set; }

        public string? DateType { get; set; }

        public string? Title { get; set; }

        public string? ShortDescription { get; set; }

        public string? StartsOn { get; set; }

        public short MinAges { get; set; }

        public short MaxAges { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public string? Location { get; set; }

        public string Ethnicity { get; set; }

        public bool? IsSpotSaved { get; set; }

        public object ToListAsync()
        {
            throw new NotImplementedException();
        }
    }
}