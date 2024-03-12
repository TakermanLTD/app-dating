using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface ICdnService
    {
        string GetPictureUrl(string bucket, string name);

        Task<IEnumerable<UserPicture>> GetUserPictures(int userId);

        Task<string> UnsetAvatar(int userId);

        Task<bool> DeletePicture(int id);

        Task SetAvatar(int userId, string name);

        Task<IEnumerable<UserPicture>> UploadUserPictures(UploadUserPicturesDto pictures);

        string GetAvatar(int userId);

        string GetDefaultAvatar();
    }
}