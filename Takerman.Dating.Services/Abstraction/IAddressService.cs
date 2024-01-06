using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IAddressService
    {
        Task<Address> GetAsync(int id);

        Task<Address> GetByUserIdAsync(int userId);

        Task RemoveAsync(int id);

        Task<Address> CreateAsync(AddressDto address);

        Task UpdateAsync(AddressDto address);
    }
}