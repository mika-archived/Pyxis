using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Alpha;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services;
using Pyxis.Services.Interfaces;

namespace Pyxis
{
    /// <summary>
    ///     既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        ///     単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///     最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        #region Overrides of PrismApplication

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                shell.SetContentFrame(rootFrame);
            else
                shell.StoreContentFrame(rootFrame);
            return shell;
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance<IPixivClient>(new PixivApiClient(), new ContainerControlledLifetimeManager());
            Container.RegisterType<IImageStoreService, ImageStoreService>(new ContainerControlledLifetimeManager());
            // Container.RegisterInstance<IPixivClient>(new PixivWebClient(), new ContainerControlledLifetimeManager());

            return base.OnInitializeAsync(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                NavigationService.Navigate("Main", null);
            return Task.CompletedTask;
        }

        #endregion
    }
}