using System.Threading.Tasks;

namespace Pyxis.Services.Interfaces
{
    public interface IDialogService
    {
        Task ShowErrorDialogAsync(string title, string content);
    }
}