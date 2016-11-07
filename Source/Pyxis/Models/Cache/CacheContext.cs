using Microsoft.EntityFrameworkCore;

namespace Pyxis.Models.Cache
{
    public class CacheContext : DbContext
    {
        public DbSet<CacheFile> CacheFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=${PyxisConstants.CacheInfoFileName}");
        }
    }
}