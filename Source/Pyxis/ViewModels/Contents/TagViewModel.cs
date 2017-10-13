using System.Diagnostics;
using System.Threading.Tasks;

using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class TagViewModel : ViewModel
    {
        private readonly Tag _tag;

        public string Tag => _tag.Name;
        public AsyncReactiveCommand NavigateCommand { get; }

        public TagViewModel(Tag tag)
        {
            _tag = tag;
            NavigateCommand = new AsyncReactiveCommand();
            NavigateCommand.Subscribe(w =>
            {
                Debug.WriteLine("Hello");
                return Task.CompletedTask;
            }).AddTo(this);
        }
    }
}