using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IUserDetail : IErrorResponse
    {
        IUser User { get; }

        IProfile Profile { get; }

        IWorkspace Workspace { get; }
    }
}