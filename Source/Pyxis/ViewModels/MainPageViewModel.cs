using System.Collections.Generic;
using System.Diagnostics;

using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Rest;

namespace Pyxis.ViewModels
{
    public class MainPageViewModel : ViewModel
    {
        private readonly IPixivClient _client;

        public string Message => "Hello, world!";

        public MainPageViewModel(IPixivClient client)
        {
            _client = client;
        }

        #region Overrides of ViewModelBase

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            var articles = await _client.Spotlight.ArticlesAsync(category => "all");
            foreach (var article in articles.SpotlightArticleList)
            {
                Debug.WriteLine(article);
            }
        }

        #endregion
    }
}