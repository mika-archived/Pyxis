using System;
using System.Threading.Tasks;

using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Extensions;
using Pyxis.Services.Interfaces;

namespace Pyxis.Controls
{
    public abstract class PixivControl<T> : Control where T : DependencyObject
    {
        private readonly IFileCacheStorage _pixivCacheStorage;
        protected T ImageControl;

        public abstract object Source { get; set; }

        protected PixivControl()
        {
            if (DesignMode.DesignModeEnabled)
                return;

            _pixivCacheStorage = PrismUnityApplication.Current.Container.Resolve<IFileCacheStorage>();
        }

        protected static void OnSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as PixivControl<T>;
            if (e.NewValue != null && e.OldValue != e.NewValue)
                control?.SetSource(e.NewValue);
        }

        protected override void OnApplyTemplate()
        {
            ImageControl = (T) GetTemplateChild("ImageControl");
            SetSource(Source);

            base.OnApplyTemplate();
        }

        private async void SetSource(object source)
        {
            if (source == null)
                return;

            var uri = source as Uri ?? (source is string ? new Uri((string) source) : null);
            if (uri == null || uri.IsHttp() && !uri.Host.EndsWith("pximg.net"))
                AssignToImageControl(uri?.ToString());
            else
                await LoadPixivImageAsync(uri.ToString());
        }

        private async Task LoadPixivImageAsync(string imageUri)
        {
            if (await _pixivCacheStorage.ExistFileAsync(imageUri))
                AssignToImageControl(await _pixivCacheStorage.LoadFileAsync(imageUri));
            else
                AssignToImageControl(await _pixivCacheStorage.SaveFileAsync(imageUri));
        }

        protected static BitmapImage CreateImageSource(string uri)
        {
            var bitmapImage = new BitmapImage(new Uri(uri));
            return bitmapImage;
        }

        protected abstract void AssignToImageControl(string image);
    }
}