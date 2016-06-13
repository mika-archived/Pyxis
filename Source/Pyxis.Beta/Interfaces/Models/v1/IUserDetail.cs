namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IUserDetail
    {
        IUser User { get; }

        IProfile Profile { get; }

        IWorkspace Workspace { get; }
    }
}