using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OptionsController(IOptionsService _optionsService, ILogger _logger) : BaseController(_logger)
    {
        [HttpGet("GetEthnicities")]
        public IEnumerable<KeyValuePair<int, string>> GetEthnicities()
        {
            return _optionsService.GetEthnicities();
        }

        [HttpGet("GetDateTypes")]
        public IEnumerable<KeyValuePair<int, string>> GetDateTypes()
        {
            return _optionsService.GetDateTypes();
        }

        [HttpGet("GetDateStatuses")]
        public IEnumerable<KeyValuePair<int, string>> GetDateStatuses()
        {
            return _optionsService.GetDateStatuses();
        }
    }
}