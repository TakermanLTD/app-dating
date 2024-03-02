using Takerman.Dating.Data;

namespace Takerman.Dating.Models.DTOs
{
    public class DateChoiceDto
    {
        public int VoteForId { get; set; }

        public List<ChoiceRadioDto> Radios { get; set; } =
        [
            new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Yes), IsChecked = false},
            new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.No), IsChecked = false},
            new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Friend), IsChecked = false}
        ];

        public string TheirChoice { get; set; } = string.Empty;

        public byte[]? Avatar { get; set; } = null;

        public string Name { get; set; } = "User";

        public string MyChoice { get; set; } = string.Empty;
    }
}