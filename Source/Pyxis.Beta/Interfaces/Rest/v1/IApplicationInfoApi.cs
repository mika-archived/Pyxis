using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IApplicationInfoApi
    {
        Task<IApplicationInfo> IosAsync();
    }
}