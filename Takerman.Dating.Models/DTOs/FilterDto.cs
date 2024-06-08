namespace Takerman.Dating.Models.DTOs
{
    public class FilterDto
    {
        public int? MinAges { get; set; }
        public int? MaxAges { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? Ethnicity { get; set; }
        public int? DateType { get; set; }
    }
}