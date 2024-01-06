namespace Takerman.Dating.Data.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int Number { get; set; }

        public string Phone { get; set; }
    }
}