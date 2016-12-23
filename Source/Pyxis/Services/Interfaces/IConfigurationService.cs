namespace Pyxis.Services.Interfaces
{
    public interface IConfigurationService
    {
        object this[string key] { get; set; }
    }
}