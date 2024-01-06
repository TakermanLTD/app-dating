using Microsoft.EntityFrameworkCore;

namespace Takerman.Dating.Data
{
    public class DefaultContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<UploadData> UploadDatas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<ResetPasswordRequest> ResetPasswordRequests { get; set; }
        public DbSet<Color> Colors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStrings.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Upload>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Color>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Upload>()
                .HasOne(x => x.UploadData)
                .WithOne(x => x.Upload)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            modelBuilder.Entity<UploadData>()
                .HasOne(x => x.Upload)
                .WithOne(x => x.UploadData)
                .HasForeignKey<Upload>(e => e.UploadDataId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}