using System.Threading.Tasks;

namespace Pyxis.ViewModels.Home
{
    public abstract class HomeContentViewModel : ViewModel
    {
        /// <summary>
        ///     When first loaded UI content, call this method.
        /// </summary>
        /// <returns></returns>
        public abstract Task InitializeAsync();
    }
}