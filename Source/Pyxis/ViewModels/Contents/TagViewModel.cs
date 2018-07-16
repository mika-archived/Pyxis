using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Pyxis.Constants;
using Pyxis.Models;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class TagViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly Tag _tag;
        private ICommand _onTappedCommand;

        public string Name => _tag.Name;
        public ICommand OnTappedCommand => _onTappedCommand ?? (_onTappedCommand = new DelegateCommand(OnTapped));

        public TagViewModel(Tag tag, INavigationService navigationService)
        {
            _tag = tag;
            _navigationService = navigationService;
        }

        private void OnTapped()
        {
            _navigationService.Navigate(PageTokens.DetailsPage, new TransitionParameter().ToQueryString());
        }
    }
}