using Takerman.Dating.Data;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IUploadService
    {
        Task<Upload> GetAsync(int id);

        Task<Upload> DownloadAsync(int id);

        Task<Upload> CreateAsync(Upload model);
        
        Task<IEnumerable<Upload>> GetByUserId(int userId);
        
        Task DeleteAsync(int id);
    }
}