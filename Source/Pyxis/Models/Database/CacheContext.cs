using Microsoft.EntityFrameworkCore;

namespace Pyxis.Models.Database
{
    internal class CacheContext : DbContext
    {
        public DbSet<Cache> Caches { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=cache.db");
        }
    }
}