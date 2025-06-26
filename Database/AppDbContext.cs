using InitialSetupBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InitialSetupBackend.Database
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetIndexColumns(modelBuilder);

            var dateNow = new DateTimeOffset(DateTime.Parse("2025-01-01"));

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "admin@joaovitordev.pro",
                Name = "Admin",
                Role = "admin",
                HashPassword = "AQAAAAIAAYagAAAAEJ4RSD8RguLszJU8rP8wWs2bhNPEPIsL0l/Bi3+NZbj1Mh59DdbBHLk/DakPMZI5Pg==", //"admin@jaodev123"
                CreatedAt = dateNow
            });

            base.OnModelCreating(modelBuilder);
        }

        private static void SetIndexColumns(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                 .HasIndex(e => new { e.Email });

            modelBuilder.Entity<User>()
                 .HasIndex(e => new { e.Name });
        }
    }
}
