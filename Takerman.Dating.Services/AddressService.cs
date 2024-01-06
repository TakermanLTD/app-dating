using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Data.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class AddressService : IAddressService
    {
        private readonly IMapper _mapper;

        public AddressService()
        {
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddressDto, Address>();
                cfg.CreateMap<Address, AddressDto>();
            }).CreateMapper();
        }

        public async Task<Address> CreateAsync(AddressDto address)
        {
            await using var context = new DefaultContext();

            var entity = _mapper.Map<Address>(address);

            var result = (await context.AddAsync(entity)).Entity;

            await context.SaveChangesAsync();

            return result;
        }

        public async Task<Address> GetAsync(int id)
        {
            await using var context = new DefaultContext();
            return await context.Addresses.FirstAsync(x => x.Id == id);
        }

        public async Task<Address> GetByUserIdAsync(int userId)
        {
            await using var context = new DefaultContext();
            return await context.Addresses.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task RemoveAsync(int id)
        {
            await using var context = new DefaultContext();
            context.Addresses.Remove(await GetAsync(id));
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AddressDto address)
        {
            await using var context = new DefaultContext();
            var original = await GetAsync(address.Id);
            original = _mapper.Map<Address>(address);
            context.Addresses.Update(original);
            await context.SaveChangesAsync();
        }
    }
}