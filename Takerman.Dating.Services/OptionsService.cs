using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class OptionsService(DefaultContext _context, ILogger<OptionsService> _logger) : IOptionsService
    {
        public IEnumerable<KeyValuePair<int, string>> GetEthnicities()
        {
            return Enum.GetValues<Ethnicity>()
                .Select(x => new KeyValuePair<int, string>((int)x, x.GetDisplay()))
                .ToList();
        }

        public IEnumerable<KeyValuePair<int, string>> GetDateTypes()
        {
            return Enum.GetValues<DateType>()
                .Select(x => new KeyValuePair<int, string>((int)x, x.GetDisplay()))
                .ToList();
        }

        public async Task<IEnumerable<UserPicture>> GetUserPictures(int userId)
        {
            try
            {
                var result = await _context.UserPictures.Where(x => x.UserId == userId).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured when getting images for user");
                throw;
            }
        }

        public async Task<IEnumerable<DatePicture>> GetDatePictures(int dateId)
        {
            return await _context.DatePictures.Where(x => x.DateId == dateId).ToListAsync();
        }

        public async Task UnsetAvatar(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.AvatarId = null;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeletePicture(int id)
        {
            var picture = await _context.UserPictures.FirstOrDefaultAsync(x => x.Id == id);
            if (picture != null)
            {
                _context.UserPictures.Remove(picture);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetAvatar(int userId, int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.AvatarId = id;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GetPicture(int id)
        {
            var result = await _context.UserPictures.FirstOrDefaultAsync(x => x.Id == id);

            return result.Data;
        }

        public async Task<IEnumerable<UserPicture>> UploadUserPictures(IEnumerable<UserPicture> pictures)
        {
            var result = new List<UserPicture>();
            foreach (var picture in pictures)
            {
                var added = _context.UserPictures.Add(new UserPicture()
                {
                    Data = picture.Data,
                    UserId = picture.UserId,
                    UploadedOn = DateTime.UtcNow
                });

                result.Add(added.Entity);
            }

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<string> GetAvatar(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            
            if (user == null || user.AvatarId.HasValue == false)
                return null;

            var result = await _context.UserPictures.FirstOrDefaultAsync(x => x.Id == user.AvatarId.Value);

            return result?.Data;
        }
    }
}