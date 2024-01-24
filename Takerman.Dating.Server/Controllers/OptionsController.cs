using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OptionsController(IOptionsService optionsService, ILogger<OptionsController> logger) : ControllerBase
    {
        private readonly IOptionsService _optionsService = optionsService;
        private readonly ILogger<OptionsController> _logger = logger;

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
    }
}