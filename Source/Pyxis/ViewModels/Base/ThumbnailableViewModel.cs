using Pyxis.Models.Base;

namespace Pyxis.ViewModels.Base
{
    public class ThumbnailableViewModel : ViewModel
    {
#pragma warning disable 169
        private bool _isRequested;
#pragma warning restore 169

        protected ThumbnailableViewModel()
        {
            ThumbnailPath = PyxisConstants.DummyImage;
        }

        #region Thumbnailable

        private ThumbnailableBase _thumbnailable;

        protected ThumbnailableBase Thumbnailable
        {
            get { return _thumbnailable; }
            set
            {
#if OFFLINE
                SetProperty(ref _thumbnailable, value);
#else
                if (SetProperty(ref _thumbnailable, value) && _isRequested)
                    RunHelper.RunLater(_thumbnailable.ShowThumbnail, TimeSpan.FromMilliseconds(100));
#endif
            }
        }

        #endregion

        #region ThumbnailPath

        private string _thumbnailPath;

        public string ThumbnailPath
        {
            get
            {
#if !OFFLINE
                if (_thumbnailPath == PyxisConstants.DummyImage || _thumbnailPath == PyxisConstants.DummyIcon)
                {
                    Thumbnailable?.ShowThumbnail();
                    _isRequested = true;
                }
#endif
                return _thumbnailPath;
            }
            set { SetProperty(ref _thumbnailPath, value); }
        }

        #endregion
    }
}