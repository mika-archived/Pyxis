using Prism.Mvvm;

namespace Pyxis.Models.Base
{
    public abstract class ThumbnailableBase : BindableBase
    {
        protected ThumbnailableBase()
        {
            ThumbnailPath = PyxisConstants.DummyImage;
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
    }
}