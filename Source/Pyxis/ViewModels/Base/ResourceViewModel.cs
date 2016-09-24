using Windows.ApplicationModel.Resources;

namespace Pyxis.ViewModels.Base
{
    public class ResourceViewModel : ViewModel
    {
        protected ResourceLoader Resources { get; }

        public ResourceViewModel()
        {
            Resources = ResourceLoader.GetForCurrentView();
        }
    }
}