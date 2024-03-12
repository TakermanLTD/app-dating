using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Takerman.Dating.Data;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class OptionsService(DefaultContext _context, ILogger<OptionsService> _logger, IOptions<CdnConfig> _cdnConfig, ICdnService _cdnService) : IOptionsService
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