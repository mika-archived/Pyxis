using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;
using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;
using Pyxis.ViewModels.Detail.Items;

namespace Pyxis.ViewModels.Detail
{
    // TODO: モバイル実機限定で、3ページ目だけ何故か表示されないことがある。
    public class MangaViewPageViewModel : ViewModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IImageStoreService _imageStoreService;

        public ObservableCollection<PixivMangaImageViewModel> MangaPages { get; }

        public MangaViewPageViewModel(ICategoryService categoryService, IImageStoreService imageStoreService)
        {
            _categoryService = categoryService;
            _imageStoreService = imageStoreService;
            MangaPages = new ObservableCollection<PixivMangaImageViewModel>();
        }

        private void Initialize(IllustDetailParameter parameter)
        {
            _categoryService.UpdateCategory();
            foreach (var item in parameter.Illust.MetaPages.Select((w, i) => new {Index = i}).Reverse())
                MangaPages.Add(new PixivMangaImageViewModel(parameter.Illust, item.Index, _imageStoreService));
            SelectedIndex = parameter.Illust.MetaPages.Count() - 1;
            MaxPage = parameter.Illust.MetaPages.Count();
            CurrentPage = 1;
        }

        #region Overrides of ViewModelBase

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);

            var parameter = ParameterBase.ToObject<IllustDetailParameter>(e.Parameter?.ToString());
            Initialize(parameter);
        }

        #endregion

        #region MaxPage

        private int _maxPage;

        public int MaxPage
        {
            get { return _maxPage; }
            set { SetProperty(ref _maxPage, value); }
        }

        #endregion

        #region CurrentPage

        private int _currentPage;

        public int CurrentPage
        {
            get { return _currentPage; }
            set { SetProperty(ref _currentPage, value); }
        }

        #endregion

        #region SelectedIndex

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (SetProperty(ref _selectedIndex, value))
                    CurrentPage = MaxPage - value;
            }
        }

        #endregion
    }
}