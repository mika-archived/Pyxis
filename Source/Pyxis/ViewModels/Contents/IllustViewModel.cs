using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Constants;
using Pyxis.Models.Parameters;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class IllustViewModel : ViewModel
    {
        private readonly Illust _illust;
        private readonly INavigationService _navigationService;

        private ICommand _onTappedCommand;

        public string Title => _illust.Title;
        public string ThumbnailUrl => _illust.ImageUrls.Medium;
        public string Username => _illust.User.Name;
        public string UserIcon => _illust.User.ProfileImageUrls.Medium;
        public ICommand OnTappedCommand => _onTappedCommand ?? (_onTappedCommand = new DelegateCommand(OnTapped));

        public IllustViewModel(Illust illust, INavigationService navigationService)
        {
            _illust = illust;
            _navigationService = navigationService;
        }

        private void OnTapped()
        {
            _navigationService.Navigate(PageTokens.DetailsPage, new DetailsParameter().ToQueryString());
        }
    }
}