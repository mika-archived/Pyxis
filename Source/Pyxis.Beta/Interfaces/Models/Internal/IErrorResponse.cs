namespace Pyxis.Beta.Interfaces.Models.Internal
{
    public interface IErrorResponse
    {
        dynamic Error { get; }

        bool HasError { get; }
    }
}