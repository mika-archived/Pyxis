using Pyxis.Models.Base;

namespace Pyxis.ViewModels.Base
{
    public class ThumbnailableViewModel : ViewModel
    {
        protected ThumbnailableBase Thumbnailable { get; set; }

        protected ThumbnailableViewModel()
        {
            ThumbnailPath = PyxisConstants.DummyImage;
        }

        #region ThumbnailPath

        private string _thumbnailPath;

        public string ThumbnailPath
        {
            get
            {
#if !OFFLINE
                if (_thumbnailPath == PyxisConstants.DummyImage)
                    Thumbnailable.ShowThumbnail();
#endif
                return _thumbnailPath;
            }
            set { SetProperty(ref _thumbnailPath, value); }
        }

        #endregion
    }
}