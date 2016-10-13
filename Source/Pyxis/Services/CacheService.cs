using System;
using System.Diagnostics;
using System.Linq;

using Pyxis.Models.Cache;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class CacheService : ICacheService
    {
        public void Create(string path, long size)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    context.CacheFiles.Add(new CacheFile {Path = path, Size = size});
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public CacheFile Reference(string path)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var cache = context.CacheFiles.Single(w => w.Path == path);
                    cache.ReferencedAt = DateTime.Now;
                    context.SaveChanges();
                    return cache;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return null;
        }

        public void Update(CacheFile cache)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == cache.Path);
                    file.Size = cache.Size;
                    file.ReferencedAt = cache.ReferencedAt;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void Delete(CacheFile cache)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == cache.Path);
                    context.CacheFiles.Remove(file);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void Clear()
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var files = context.CacheFiles.ToList();
                    foreach (var file in files)
                        context.CacheFiles.Remove(file);
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}