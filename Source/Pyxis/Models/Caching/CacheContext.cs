using Microsoft.EntityFrameworkCore;

namespace Pyxis.Models.Caching
{
    public class CacheContext : DbContext
    {
        public DbSet<CacheFile> CacheFiles { get; set; }

        public DbSet<CacheRule> CacheRules { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Cache.db");
        }
    }
}