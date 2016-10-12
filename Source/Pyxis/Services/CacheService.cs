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
                    return context.CacheFiles.Single(w => w.Path == path);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return null;
        }

        public void Update(string path, long size)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == path);
                    file.Size = size;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void Delete(string path)
        {
            using (var context = new CacheContext())
            {
                try
                {
                    var file = context.CacheFiles.Single(w => w.Path == path);
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