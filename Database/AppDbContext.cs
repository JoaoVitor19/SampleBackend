using InitialSetupBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace InitialSetupBackend.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
