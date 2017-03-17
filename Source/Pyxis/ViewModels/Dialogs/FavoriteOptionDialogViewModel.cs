using Pyxis.Collections;
using Pyxis.Helpers;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using Sagitta;

namespace Pyxis.ViewModels.Dialogs
{
    public class FavoriteOptionDialogViewModel : DialogViewModel
    {
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;

        private PixivBookmarkTag _bookmarkTag;
        private FavoriteOptionParameter _parameter;

        public ReactiveProperty<RestrictType> RestrictType { get; private set; }
        public ReactiveProperty<string> SelectedTag { get; private set; }
        public IncrementalObservableCollection<string> Tags { get; }

        public FavoriteOptionDialogViewModel(PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
            Tags = new IncrementalObservableCollection<string>();
        }

        #region Overrides of DialogViewModel

        public override void OnInitialize(object parameter)
        {
            _parameter = parameter as FavoriteOptionParameter ?? new FavoriteOptionParameter();
            RestrictType = _parameter.ToReactivePropertyAsSynchronized(w => w.Restrict);
            SelectedTag = _parameter.ToReactivePropertyAsSynchronized(w => w.Tag);
            _bookmarkTag = new PixivBookmarkTag(_pixivClient, _queryCacheService);
            ModelHelper.ConnectTo(Tags, _bookmarkTag, w => w.BookmarkTags, w => w.Name).AddTo(this);

            _bookmarkTag.Query(_parameter.Type, _parameter.Restrict);
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion
    }
}