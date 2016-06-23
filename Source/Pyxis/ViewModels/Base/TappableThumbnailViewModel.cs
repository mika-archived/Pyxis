using System.Diagnostics;

namespace Pyxis.ViewModels.Base
{
    public class TappableThumbnailViewModel : ThumbnailableViewModel
    {
        public virtual void OnItemTapped()
        {
            Debug.WriteLine("OnTapped");
            // Nothing to do
        }
    }
}