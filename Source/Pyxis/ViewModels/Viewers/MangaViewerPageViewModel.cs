using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

using Pyxis.Models.Parameters;
using Pyxis.Models.Pixiv;
using Pyxis.Mvvm;
using Pyxis.Navigation;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Contents;

using Reactive.Bindings.Extensions;

using Sagitta.Models;

using WinRTXamlToolkit.Tools;

namespace Pyxis.ViewModels.Viewers
{
    public class MangaViewerPageViewModel : ViewModel
    {
        private readonly PixivPostDetail<Illust> _postDetail;
        public ObservableCollection<SingleIllustPageViewModel> OriginalImageUris { get; }

        public MangaViewerPageViewModel()
        {
            OriginalImageUris = new ObservableCollection<SingleIllustPageViewModel>();
            SelectedIndex = 0;
            _postDetail = new PixivPostDetail<Illust>(null);
            _postDetail.ObserveProperty(w => w.Post)
                       .Where(w => w != null)
                       .Subscribe(w =>
                       {
                           OriginalImageUris.Clear();
                           Enumerable.Range(1, w.PageCount).ForEach(v => OriginalImageUris.Add(new SingleIllustPageViewModel(w, v)));
                       })
                       .AddTo(this);
        }

        public override void OnNavigatedTo(PyxisNavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            var parameter = e.ParsedQuery<IllustParameter>();
            _postDetail.ApplyForce(parameter.Illust);
        }

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        #endregion
    }
}