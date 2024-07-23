using Microsoft.AspNetCore.Http;
using Takerman.Dating.Data;

namespace Takerman.Dating.Services.Abstraction
{
    public interface ICdnService
    {
        Task<IEnumerable<UserPicture>> GetUserPictures(int userId);

        Task<EthnicPicture> GetDateThumbnail();

        Task<IEnumerable<EthnicPicture>> GetEthnicPictures();

        Task<bool> DeletePicture(int id);

        Task SetAvatar(int userId, string url);

        Task<IEnumerable<UserPicture>> UploadUserPictures(int userId, IEnumerable<IFormFile> files);

        string GetAvatar(int userId);
        
        Task<string> SetDefaultAvatar(int userId);

    }
}