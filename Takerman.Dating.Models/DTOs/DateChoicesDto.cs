namespace Takerman.Dating.Models.DTOs
{
    public class DateChoicesDto
    {
        public List<ChoiceRadioDto> Radios { get; set; }

        public string TheirChoice { get; set; } = string.Empty;
        
        public byte[]? Avatar { get; set; } = null;
        public string Name { get; set; }
    }
}