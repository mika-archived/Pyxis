using Prism.Windows.Navigation;

using Pyxis.Constants;
using Pyxis.Models.Parameters;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class IllustViewModel : ContentViewModel
    {
        public IllustViewModel(Illust illust, INavigationService navigationService) : base(illust, navigationService) { }

        protected override void OnTapped()
        {
            var parameter = new DetailsParameter
            {
                Type = typeof(Illust),
                Work = Work
            };
            NavigationService.Navigate(PageTokens.DetailsPage, parameter.ToQueryString());
        }
    }
}