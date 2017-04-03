using System;
using System.Collections.Generic;
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

namespace Pyxis.ViewModels.Viewers
{
    public class MangaViewerPageViewModel : ViewModel
    {
        private readonly PixivPostDetail<Illust> _postDetail;
        public List<SingleIllustPageViewModel> OriginalImageUris { get; }

        public MangaViewerPageViewModel()
        {
            OriginalImageUris = new List<SingleIllustPageViewModel>();
            SelectedIndex = 0;
            _postDetail = new PixivPostDetail<Illust>(null);
            _postDetail.ObserveProperty(w => w.Post)
                       .Where(w => w != null)
                       .Subscribe(w =>
                       {
                           OriginalImageUris.Clear();
                           for (var i = 0; i < w.MetaPages.Count(); i++)
                               OriginalImageUris.Add(new SingleIllustPageViewModel(w, i + 1));
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

        #region SelectedItem

        private SingleIllustPageViewModel _selectedItem;

        public SingleIllustPageViewModel SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        #endregion
    }
}