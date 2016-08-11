using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

namespace Pyxis.Services.Interfaces
{
    public interface IDialogService
    {
        Task<ContentDialogResult> ShowDialogAsync(string dialogToken);

        Task<object> ShowDialogAsync(string dialogToken, object parameter);
    }
}