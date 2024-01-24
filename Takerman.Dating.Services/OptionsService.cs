using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class OptionsService : IOptionsService
    {
        public IEnumerable<KeyValuePair<int, string>> GetEthnicities()
        {
            return Enum.GetValues<Ethnicity>()
                .Select(x => new KeyValuePair<int, string>((int)x, x.GetDisplay()))
                .ToList();
        }

        public IEnumerable<KeyValuePair<int, string>> GetDateTypes()
        {
            return Enum.GetValues<DateType>()
                .Select(x => new KeyValuePair<int, string>((int)x, x.GetDisplay()))
                .ToList();
        }
    }
}