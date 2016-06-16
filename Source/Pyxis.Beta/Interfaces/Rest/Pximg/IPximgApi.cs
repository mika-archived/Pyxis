using System.IO;
using System.Threading.Tasks;

namespace Pyxis.Beta.Interfaces.Rest.Pximg
{
    public interface IPximgApi
    {
        Task<Stream> GetAsync(string url);
    }
}