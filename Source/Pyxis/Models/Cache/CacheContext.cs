using Windows.Storage;

using Microsoft.EntityFrameworkCore;

using Pyxis.Extensions;

namespace Pyxis.Models.Cache
{
    public class CacheContext : DbContext
    {
        public DbSet<CacheFile> CacheFiles { get; set; }

        public bool IsCreated
        {
            get
            {
                var localState = ApplicationData.Current.LocalFolder;
                return localState.GetFileWhenNotFoundReturnNullAsync("$cache.db").Result != null;
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=${PyxisConstants.CacheInfoFileName}");
        }
    }
}