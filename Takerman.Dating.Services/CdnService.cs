using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Takerman.Dating.Data;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Takerman.Dating.Services
{
    public class CdnService : ICdnService
    {
        private readonly Cloudinary _cdn;
        private readonly ILogger<CdnService> _logger;
        private readonly DefaultContext _context;
        private readonly IOptions<CdnConfig> _cdnConfig;

        public CdnService(DefaultContext context, IOptions<CdnConfig> cdnConfig, ILogger<CdnService> logger)
        {
            _logger = logger;
            _context = context;
            _cdnConfig = cdnConfig;
            _cdn = new Cloudinary($"cloudinary://{_cdnConfig.Value.CloudinaryApiKey}:{_cdnConfig.Value.CloudinaryApiSecret}@{_cdnConfig.Value.CloudinaryApiArea}");
            _cdn.Api.Secure = true;
        }

        public string GetPictureUrl(string bucket, string name)
        {
            return $"https://res.cloudinary.com/{_cdnConfig.Value.CloudinaryApiArea}/image/upload/v1710183279/dating/{bucket}/{name}";
        }

        public async Task<IEnumerable<UserPicture>> GetUserPictures(int userId)
        {
            try
            {
                var pictures = await _context.UserPictures.Where(x => x.UserId == userId).OrderBy(x => x.UploadedOn).ToListAsync();

                var result = new List<UserPicture>();

                foreach (var picture in pictures)
                {
                    var url = GetPictureUrl(_cdnConfig.Value.ProfilesBucket, picture.Name);
                    picture.Name = url;
                    result.Add(picture);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured when getting images for user");
                throw;
            }
        }

        public async Task<string> UnsetAvatar(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.Avatar = null;

                await _context.SaveChangesAsync();
            }

            return GetDefaultAvatar();
        }

        public async Task<bool> DeletePicture(int id)
        {
            var picture = await _context.UserPictures.FirstOrDefaultAsync(x => x.Id == id);
            if (picture != null)
            {
                _context.UserPictures.Remove(picture);

                await _context.SaveChangesAsync();

                return true;
            }

            try
            {
                var result = await _cdn.DeleteResourcesAsync(picture.Name);

                return result.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured when deleting an image");

                return false;
            }
        }

        public async Task SetAvatar(int userId, string url)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                user.Avatar = url;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<UserPicture>> UploadUserPictures(UploadUserPicturesDto picture)
        {
            var result = new List<UserPicture>();
            var uploadParams = new ImageUploadParams()
            {
                Folder = $"dating/{_cdnConfig.Value.ProfilesBucket}",
            };

            var fileName = Guid.NewGuid().ToString() + ".png";
            using var stream = new MemoryStream(picture.Picture);

            try
            {
                uploadParams.File = new FileDescription(fileName, stream);
                var upload = _cdn.Upload(uploadParams);

                if (upload.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError($"An exception occured when uploading image to the CDN. Status code {upload.StatusCode} {upload.Error?.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured when uploading image to the CDN");
            }

            var added = _context.UserPictures.Add(new UserPicture()
            {
                Name = fileName,
                UserId = picture.UserId,
                UploadedOn = DateTime.UtcNow
            });

            result.Add(added.Entity);

            await _context.SaveChangesAsync();

            return result;
        }

        public string GetAvatar(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            if (!string.IsNullOrEmpty(user.Avatar))
            {
                return GetPictureUrl(_cdnConfig.Value.ProfilesBucket, user.Avatar);
            }
            else
            {
                return GetDefaultAvatar();
            }
        }

        public string GetDefaultAvatar()
        {
            return GetPictureUrl(_cdnConfig.Value.ProfilesBucket, _cdnConfig.Value.ProfileDefaultAvatar);
        }
    }
}