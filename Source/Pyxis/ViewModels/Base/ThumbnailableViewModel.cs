using System;

using Pyxis.Helpers;
using Pyxis.Models.Base;

namespace Pyxis.ViewModels.Base
{
    public class ThumbnailableViewModel : ViewModel
    {
        private bool _isRequested;

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
                if (SetProperty(ref _thumbnailable, value) && _isRequested)
#if !OFFLINE
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