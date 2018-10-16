using System.Linq;
using System.Windows.Input;

using Prism.Commands;
using Prism.Windows.Navigation;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class ContentViewModel : ViewModel
    {
        private ICommand _onTappedCommand;
        protected INavigationService NavigationService { get; }
        protected Work Work { get; }

        public string Title => Work.Title;
        public string Description => Work.Caption;
        public string ThumbnailUrl => Work.ImageUrls.Medium;
        public object Tags => Work.Tags.Select(w => w).ToList();
        public string TagLine => string.Join(", ", Work.Tags.Select(w => w.Name).ToList());
        public string Username => Work.User.Name;
        public string UserIcon => Work.User.ProfileImageUrls.Medium;
        public ICommand OnTappedCommand => _onTappedCommand ?? (_onTappedCommand = new DelegateCommand(OnTapped));

        protected ContentViewModel(Work work, INavigationService navigationService)
        {
            Work = work;
            NavigationService = navigationService;
        }

        protected virtual void OnTapped() { }
    }
}