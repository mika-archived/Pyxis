using System.Threading.Tasks;

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;

using Microsoft.Practices.Unity;

using Prism.Unity.Windows;
using Prism.Windows.AppModel;

using Pyxis.Alpha;
using Pyxis.Beta.Interfaces.Rest;

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

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            Container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));
            Container.RegisterInstance<IPixivClient>(new PixivApiClient(), new ContainerControlledLifetimeManager());
            // Container.RegisterInstance<IPixivClient>(new PixivWebClient(), new ContainerControlledLifetimeManager());

            return base.OnInitializeAsync(args);
        }

        #endregion

        #region Overrides of PrismApplication

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
            return Task.CompletedTask;
        }

        #endregion
    }
}