using System.Collections.Generic;
using System.Reactive.Linq;

using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta.Models;

namespace Pyxis.ViewModels.Viewers
{
    public class IllustViewerPageViewModel : ViewModel
    {
        private readonly PixivPostDetail<Illust> _postDetail;
        public ReadOnlyReactiveProperty<SingleIllustPageViewModel> PageViewModel { get; }

        public IllustViewerPageViewModel()
        {
            _postDetail = new PixivPostDetail<Illust>(null);
            PageViewModel = _postDetail.ObserveProperty(w => w.Post)
                                       .Where(w => w != null)
                                       .Select(w => new SingleIllustPageViewModel(w, 1))
                                       .ToReadOnlyReactiveProperty()
                                       .AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<IllustParameter>();
            _postDetail.ApplyForce(parameter.Illust);
        }
    }
}