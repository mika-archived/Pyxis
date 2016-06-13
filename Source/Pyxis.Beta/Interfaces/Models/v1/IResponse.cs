namespace Pyxis.Beta.Interfaces.Models.v1
{
    /// <summary>
    ///     認証情報
    ///     ** API only
    /// </summary>
    public interface IResponse
    {
        string AccessToken { get; }

        int ExpiresIn { get; }

        string TokenType { get; }

        string Scope { get; }

        string RefreshToken { get; }

        IAccount User { get; }

        string DeviceToken { get; }
    }
}