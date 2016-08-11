using System;
using System.Collections.Generic;
using System.Reactive.Linq;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Detail
{
    public class NovelViewPageViewModel : ViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IPixivClient _pixivClient;
        private PixivNovelText _pixivNovelText;

        public NovelViewPageViewModel(ICategoryService categoryService, IPixivClient pixivClient)
        {
            _categoryService = categoryService;
            _pixivClient = pixivClient;
            Text = "読込中...";
        }

        private void Initialize(NovelDetailParameter parameter)
        {
            _categoryService.UpdateCategory();
            var novel = parameter.Novel;
            _pixivNovelText = new PixivNovelText(novel, _pixivClient);
            _pixivNovelText.ObserveProperty(w => w.Text)
                           .Where(w => w != null)
                           .ObserveOnUIDispatcher()
                           .Subscribe(w => Text = w.NovelText)
                           .AddTo(this);
#if !OFFLINE
            _pixivNovelText.Fetch();
#endif
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var parameter = ParameterBase.ToObject<NovelDetailParameter>((string) e.Parameter);
            Initialize(parameter);
        }

        #endregion

        #region Text

        private string _text;

        public string Text
        {
            get { return _text; }
            set { SetProperty(ref _text, value); }
        }

        #endregion
    }
}