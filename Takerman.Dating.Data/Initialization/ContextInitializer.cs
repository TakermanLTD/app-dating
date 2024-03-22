using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Takerman.Dating.Data.Initialization
{
    public class ContextInitializer(ILogger<ContextInitializer> _logger, DefaultContext _context) : IContextInitializer
    {
        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.IsSqlServer())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database.");
                throw;
            }
        }
    }
}