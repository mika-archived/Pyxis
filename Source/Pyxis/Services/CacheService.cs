using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Pyxis.Models.Cache;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class CacheService : ICacheService
    {
        public async Task CreateAsync(string path, long size)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    context.CacheFiles.Add(new CacheFile {Path = path, Size = size});
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public async Task<CacheFile> ReferenceAsync(string path)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var cache = context.CacheFiles.Single(w => w.Path == path);
                    cache.ReferencedAt = DateTime.Now;
                    await context.SaveChangesAsync();
                    return cache;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return null;
        }

        public async Task UpdateAsync(CacheFile cache)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == cache.Path);
                    file.Size = cache.Size;
                    file.ReferencedAt = cache.ReferencedAt;
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public async Task DeleteAsync(CacheFile cache)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == cache.Path);
                    context.CacheFiles.Remove(file);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public async Task ClearAsync()
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var files = context.CacheFiles.ToList();
                    foreach (var file in files)
                        context.CacheFiles.Remove(file);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}