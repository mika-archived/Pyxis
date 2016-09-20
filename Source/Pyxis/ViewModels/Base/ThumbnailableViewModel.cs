using System;
using System.Reactive.Linq;

using Pyxis.Models.Base;
using Pyxis.Mvvm;

using Reactive.Bindings.Extensions;

#if !OFFLINE

using Pyxis.Helpers;

#endif

namespace Pyxis.ViewModels.Base
{
    public class ThumbnailableViewModel : ViewModel
    {
        private bool _isAttached;
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
                if (!SetProperty(ref _thumbnailable, value))
                    return;
                if (!_isAttached)
                {
                    _thumbnailable.ObserveProperty(w => w.ThumbnailPath)
                                  .Where(w => !string.IsNullOrWhiteSpace(w))
                                  .ObserveOnUIDispatcher()
                                  .Subscribe(w => ThumbnailPath = w)
                                  .AddTo(this);
                    _thumbnailable.ObserveProperty(w => w.IsProgress)
                                  .ObserveOnUIDispatcher()
                                  .Subscribe(w => IsProgress = w)
                                  .AddTo(this);
                    _isAttached = true;
                }
                if (_isRequested)
                    RunHelper.RunLaterUI(_thumbnailable.ShowThumbnail, TimeSpan.FromMilliseconds(100));
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
                if ((_thumbnailPath == PyxisConstants.DummyImage) || (_thumbnailPath == PyxisConstants.DummyIcon))
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