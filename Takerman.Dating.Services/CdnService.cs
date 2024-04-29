using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using Takerman.Dating.Data;
using Takerman.Dating.Models.Configuration;
using Takerman.Dating.Services.Abstraction;

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
            _cdn = new Cloudinary($"cloudinary://{_cdnConfig.Value.CloudinaryApiKey}:{_cdnConfig.Value.CloudinaryApiSecret}@takerman");
            _cdn.Api.Secure = true;
        }

        public async Task<IEnumerable<UserPicture>> GetUserPictures(int userId)
        {
            try
            {
                return await _context.UserPictures.Where(x => x.UserId == userId).OrderBy(x => x.UploadedOn).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occured when getting images for user");
                throw;
            }
        }

        public async Task<bool> DeletePicture(int id)
        {
            var picture = await _context.UserPictures.FirstOrDefaultAsync(x => x.Id == id);
            if (picture != null)
            {
                _context.UserPictures.Remove(picture);

                await _context.SaveChangesAsync();
            }

            try
            {
                var result = await _cdn.DestroyAsync(new DeletionParams(picture.PublicId));

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

        public async Task<IEnumerable<UserPicture>> UploadUserPictures(int userId, IEnumerable<IFormFile> files)
        {
            var result = new List<UserPicture>();
            var uploadParams = new ImageUploadParams()
            {
                Folder = $"dating/profiles",
            };

            foreach (var file in files)
            {
                try
                {
                    var fileName = Guid.NewGuid().ToString() + ".png";
                    using var stream = file.OpenReadStream();

                    uploadParams.File = new FileDescription(fileName, stream);
                    ImageUploadResult upload = _cdn.Upload(uploadParams);

                    if (upload.StatusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError($"An exception occured when uploading image to the CDN. Status code {upload.StatusCode} {upload.Error?.Message}");
                    }


                    var added = _context.UserPictures.Add(new UserPicture()
                    {
                        PublicId = upload.PublicId,
                        Url = upload.SecureUrl.AbsoluteUri,
                        UserId = userId,
                        UploadedOn = upload.CreatedAt
                    });

                    result.Add(added.Entity);

                    await _context.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An exception occured when uploading image to the CDN");
                }
            }

            return result;
        }

        public string GetAvatar(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);

            return string.IsNullOrEmpty(user.Avatar) ? _cdnConfig.Value.DefaultAvatarUrl : user.Avatar;
        }

        public async Task<string> SetDefaultAvatar(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == userId);
            user.Avatar = _cdnConfig.Value.DefaultAvatarUrl;
            await _context.SaveChangesAsync();
            return _cdnConfig.Value.DefaultAvatarUrl;
        }

        public async Task<IEnumerable<EthnicPicture>> GetDatePictures(Ethnicity ethnicity)
        {
            try
            {
                return await _context.EthnicPictures.Where(x => x.Ethnicity == ethnicity).OrderBy(x => x.UploadedOn).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred when getting images for user");
                throw;
            }
        }

        public async Task<EthnicPicture> GetDateThumbnail(Ethnicity ethnicity)
        {
            try
            {
                return await _context.EthnicPictures.FirstOrDefaultAsync(x => x.Ethnicity == ethnicity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An exception occurred when getting images for user");
                throw;
            }
        }
    }
}