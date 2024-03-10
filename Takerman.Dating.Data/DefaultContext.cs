using Microsoft.EntityFrameworkCore;

namespace Takerman.Dating.Data
{
    public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
    {
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Date> Dates { get; set; }
        public DbSet<DateUserChoice> DateUserChoices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPicture> UserPictures { get; set; }
        public DbSet<DatePicture> DatePictures { get; set; }
        public DbSet<UserSavedSpot> UserSavedSpots { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DatePicture>().Property(x => x.Data).HasMaxLength(int.MaxValue);
            modelBuilder.Entity<UserPicture>().Property(x => x.Data).HasMaxLength(int.MaxValue);

            base.OnModelCreating(modelBuilder);
        }
    }
}