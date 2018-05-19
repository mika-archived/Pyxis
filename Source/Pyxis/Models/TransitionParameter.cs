using Newtonsoft.Json;

using Prism.Windows.Navigation;

using Pyxis.Enums;

namespace Pyxis.Models
{
    public class TransitionParameter
    {
        public TransitionMode Mode { get; set; }

        public static T FromQueryString<T>(string queryString)
        {
            return JsonConvert.DeserializeObject<T>(queryString);
        }

        public string ToQueryString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void ProcessTransitionHistory(INavigationService navigationService)
        {
            if (Mode == TransitionMode.LoginRedirect)
                navigationService.RemoveAllPages();
        }
    }
}