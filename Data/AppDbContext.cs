using Microsoft.EntityFrameworkCore;
using FrightNight.Models;

namespace FrightNight.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Pictures> Pictures { get; set; }
        public DbSet<Memorys> Memorys { get; set; }
        public DbSet<Comments> Comments { get; set; }
    }
}