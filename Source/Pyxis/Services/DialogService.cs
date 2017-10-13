using System;
using System.Threading.Tasks;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels;
using Pyxis.Views;

namespace Pyxis.Services
{
    internal class DialogService : IDialogService
    {
        public async Task ShowErrorDialogAsync(string title, string content)
        {
            var dialog = new ErrorDialog {DataContext = new ErrorDialogViewModel {Title = title, Content = content}};
            await dialog.ShowAsync();
        }
    }
}