using Newtonsoft.Json;

using Prism.Windows.Navigation;

using Pyxis.Enums;
using Pyxis.Exceptions;

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
            if (!Validate())
                throw new InvalidParametersException();
            return JsonConvert.SerializeObject(this);
        }

        public void ProcessTransitionHistory(INavigationService navigationService)
        {
            if (Mode == TransitionMode.LoginRedirect)
                navigationService.RemoveAllPages();
        }

        protected virtual bool Validate()
        {
            return true;
        }
    }
}