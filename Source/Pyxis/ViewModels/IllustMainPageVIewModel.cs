using System.Collections.Generic;
using System.Diagnostics;

using Prism.Windows.Navigation;

using Pyxis.Services.Interfaces;

namespace Pyxis.ViewModels
{
    public class IllustMainPageViewModel : ViewModel
    {
        private readonly IImageStoreService _imageStoreService;
        public string Messaege => "Hello, world!";

        public IllustMainPageViewModel(IImageStoreService imageStoreService)
        {
            _imageStoreService = imageStoreService;
            Path = "http://placehold.jp/150x50.png";
        }

        #region Overrides of ViewModelBase

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Path =
                await
                    _imageStoreService.SaveImageAsync(
                                                      "https://i.pximg.net/c/540x540_70/img-master/img/2016/05/05/23/28/07/56731592_p0_master1200.jpg");
            Debug.WriteLine(Path);
        }

        #endregion

        #region Path

        private string _path;

        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value); }
        }

        #endregion
    }
}