using Microsoft.EntityFrameworkCore;

namespace Pyxis.Models.Caching
{
    public class CacheContext : DbContext
    {
        public DbSet<CacheFile> CacheFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Cache.db");
        }
    }
}