using Prism.Windows.Navigation;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.ViewModels.Base;

namespace Pyxis.ViewModels.Items
{
    public class PixivTagViewModel : ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly ITag _tag;

        public string Name => _tag.Name;

        public PixivTagViewModel(ITag tag, INavigationService navigationService)
        {
            _tag = tag;
            _navigationService = navigationService;
        }
    }
}