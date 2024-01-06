using Microsoft.EntityFrameworkCore;
using Takerman.Dating.Data;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class UploadService : IUploadService
    {
        public async Task<Upload> CreateAsync(Upload model)
        {
            await using var context = new DefaultContext();
            var result = (await context.Uploads.AddAsync(model)).Entity;
            await context.SaveChangesAsync();
            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await using var context = new DefaultContext();
            var upload = await context.Uploads.FirstAsync(x => x.Id == id);
            context.Uploads.Remove(upload);
            await context.SaveChangesAsync();
        }

        public async Task<Upload> DownloadAsync(int id)
        {
            await using var context = new DefaultContext();
            var model = await context.Uploads.Include(x => x.UploadData).FirstAsync(x => x.Id == id);
            model.Downloads++;
            context.Uploads.Update(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<Upload> GetAsync(int id)
        {
            await using var context = new DefaultContext();
            return await context.Uploads.FirstAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Upload>> GetByUserId(int userId)
        {
            await using var context = new DefaultContext();
            return await context.Uploads.Where(x => x.UserId == userId).ToListAsync();
        }
    }
}