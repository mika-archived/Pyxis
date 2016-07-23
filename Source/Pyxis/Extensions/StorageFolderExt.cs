using System;
using System.Threading.Tasks;

using Windows.Storage;

namespace Pyxis.Extensions
{
    public static class StorageFolderExt
    {
        public static async Task<StorageFolder> GetFolderWhenNotFoundReturnNullAsync(this StorageFolder obj, string name)
        {
            try
            {
                return await obj.GetFolderAsync(name);
            }
            catch
            {
                return null;
            }
        }
    }
}