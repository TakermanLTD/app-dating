using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class SpecificationService : ISpecificationService
    {
        public async Task<Color> GetColorById(int id)
        {
            await using var context = new DefaultContext();
            return await context.Colors.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Color> GetColorByName(string name)
        {
            await using var context = new DefaultContext();
            return await context.Colors.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<Color>> GetColorsAsync()
        {
            await using var context = new DefaultContext();
            return await context.Colors.ToListAsync();
        }

        public async Task<IEnumerable<Color>> GetAvailableColorsAsync()
        {
            await using var context = new DefaultContext();
            return await context.Colors.Where(x => x.Available == true).ToListAsync();
        }

        public async Task<Color> SetColorAvailable(string name, bool available)
        {
            await using var context = new DefaultContext();
            var color = await GetColorByName(name);
            if (color != null)
            {
                color.Available = available;
                context.Colors.Update(color);
                await context.SaveChangesAsync();
            }
            return color;
        }
    }
}