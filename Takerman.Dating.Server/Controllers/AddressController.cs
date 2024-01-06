using Microsoft.AspNetCore.Mvc;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController(ILogger<AddressController> logger, IAddressService addressService) : ControllerBase
    {
        private readonly IAddressService _addressService = addressService;
        private readonly ILogger<AddressController> _logger = logger;

        [HttpPost("Create")]
        public async Task<Address> Create(AddressDto address)
        {
            return await _addressService.CreateAsync(address);
        }

        [HttpPut("Update")]
        public async Task Update(AddressDto address)
        {
            await _addressService.UpdateAsync(address);
        }

        [HttpGet("GetByUserId")]
        public async Task<Address> GetByUserId(int id)
        {
            var address = await _addressService.GetByUserIdAsync(id);

            var result = address ?? new Address();

            return result;
        }
    }
}