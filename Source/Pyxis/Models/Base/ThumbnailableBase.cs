using Prism.Mvvm;

namespace Pyxis.Models.Base
{
    public abstract class ThumbnailableBase : BindableBase
    {
        protected ThumbnailableBase()
        {
            IsProgress = true;
        }

        public abstract void ShowThumbnail();

        #region ThumbnailPath

        private string _thumbnailPath;

        public string ThumbnailPath
        {
            get { return _thumbnailPath; }
            set { SetProperty(ref _thumbnailPath, value); }
        }

        #endregion

        #region IsProgress

        private bool _isProgress;

        public bool IsProgress
        {
            get { return _isProgress; }
            set { SetProperty(ref _isProgress, value); }
        }

        #endregion
    }
}