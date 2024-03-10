using Takerman.Dating.Data;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IOptionsService
    {
        IEnumerable<KeyValuePair<int, string>> GetEthnicities();

        IEnumerable<KeyValuePair<int, string>> GetDateTypes();

        Task<IEnumerable<UserPicture>> GetUserPictures(int userid);

        Task UnsetAvatar(int userId);

        Task DeletePicture(int id);

        Task<string> GetPicture(int id);

        Task<string> SetAvatar(int userId, int id);

        Task<IEnumerable<DatePicture>> GetDatePictures(int dateId);

        Task<IEnumerable<UserPicture>> UploadUserPictures(IEnumerable<UserPicture> pictures);
        
        Task<string> GetAvatar(int userId);
    }
}