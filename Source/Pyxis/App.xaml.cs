using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;

using Pyxis.Alpha;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Services;
using Pyxis.Services.Interfaces;

using Reactive.Bindings;

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
            UnhandledException += (sender, e) =>
            {
                Debug.WriteLine("");
                Debug.WriteLine(e.Message);

                e.Handled = true;
                Application.Current.Exit();
            };
        }

        #region Overrides of PrismApplication

        protected override Task OnActivateApplicationAsync(IActivatedEventArgs e)
        {
            var args = e as ProtocolActivatedEventArgs;
            if (args == null)
                return Task.CompletedTask;

            PyxisSchemeActivator.Activate(args.Uri, NavigationService);
            return Task.CompletedTask;
        }

        #endregion

        #region Overrides of PrismApplication

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<AppShell>();
            shell.SetContentFrame(rootFrame);
            return shell;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            UIDispatcherScheduler.Initialize();

            var pixivClient = new PixivApiClient();
            var accountService = new AccountService(pixivClient);

            Container.RegisterInstance<IPixivClient>(pixivClient, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IAccountService>(accountService, new ContainerControlledLifetimeManager());
            Container.RegisterType<IImageStoreService, ImageStoreService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            // Container.RegisterInstance<IPixivClient>(new PixivWebClient(), new ContainerControlledLifetimeManager());
#if !OFFLINE
            await accountService.Login();
#endif
            await base.OnInitializeAsync(args);
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("HomeMain", "{\"ContentType\":0}");
            var param = new WorkParameter {ContentType = ContentType.Illust};
            Debug.WriteLine(param.ToJson());
            return Task.CompletedTask;
        }

        #endregion
    }
}