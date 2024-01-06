using Takerman.Dating.Data;

namespace Takerman.Dating.Services.Abstraction
{
    public interface ISpecificationService
    {
        Task<Color> GetColorById(int id);

        Task<Color> GetColorByName(string name);

        Task<IEnumerable<Color>> GetColorsAsync();

        Task<IEnumerable<Color>> GetAvailableColorsAsync();

        Task<Color> SetColorAvailable(string name, bool available);
    }
}