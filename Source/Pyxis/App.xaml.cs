using System;
using System.Globalization;
using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.Practices.Unity;

using Prism.Mvvm;
using Prism.Unity.Windows;
using Prism.Windows.AppModel;

using Pyxis.Constants;
using Pyxis.Services;
using Pyxis.Services.Interfaces;
using Pyxis.Views;

using PyxisControls;
using PyxisControls.Services;

using Sagitta;

namespace Pyxis
{
    /// <summary>
    ///     既定の Application クラスを補完するアプリケーション固有の動作を提供します。
    /// </summary>
    public sealed partial class App : PrismUnityApplication
    {
        /// <summary>
        ///     単一アプリケーション オブジェクトを初期化します。これは、実行される作成したコードの
        ///     最初の行であるため、main() または WinMain() と論理的に等価です。
        /// </summary>
        public App()
        {
            InitializeComponent();
            AppCenter.Start(PyxisConstants.AppCenterId, typeof(Analytics));
        }

        protected override void ConfigureContainer()
        {
            // register a singleton using Container.RegisterType<IInterface, Type>(new ContainerControlledLifetimeManager());
            base.ConfigureContainer();

            var pixivClient = new PixivClient(PyxisConstants.PixivClientId, PyxisConstants.PixivClientSecret);
            var cacheStorage = new PixivCacheStorage(pixivClient);
            PyxisInjector.Instance.Register<IFileCacheStorage>(cacheStorage);

            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterInstance(pixivClient, new ContainerControlledLifetimeManager());
            Container.RegisterInstance<IFileCacheStorage>(cacheStorage, new ContainerControlledLifetimeManager());
            Container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IDialogService, DialogService>(new ContainerControlledLifetimeManager());
            Container.RegisterType<IObjectCacheStorage, SessionObjectCacheStorage>(new ContainerControlledLifetimeManager());
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            var accountService = Container.Resolve<IAccountService>();
            await accountService.LoginAsync();
            await LaunchApplicationAsync(accountService.CurrentUser == null ? PageTokens.LoginPage : PageTokens.HomePage, null);
        }

        private async Task LaunchApplicationAsync(string page, object launchParam)
        {
            ThemeSelectorService.SetRequestedTheme();
            NavigationService.Navigate(page, launchParam);
            Window.Current.Activate();
            await Task.CompletedTask;
        }

        protected override async Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // We are remapping the default ViewNamePage and ViewNamePageViewModel naming to ViewNamePage and ViewNameViewModel to
            // gain better code reuse with other frameworks and pages within Windows Template Studio
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "Pyxis.ViewModels.{0}ViewModel, Pyxis",
                                                      viewType.Name.Substring(0, viewType.Name.Length - 4));
                return Type.GetType(viewModelTypeName);
            });
            await base.OnInitializeAsync(args);
        }

        protected override UIElement CreateShell(Frame rootFrame)
        {
            var shell = Container.Resolve<ShellPage>();
            shell.SetRootFrame(rootFrame);
            return shell;
        }
    }
}