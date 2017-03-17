using System;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;

namespace Pyxis.Models
{
    // +=================================================+
    // | Custom URI Scheme                               |
    // +---------------+---------------------------------+
    // | Open a illust | pyxis://illusts/?id=12345       |
    // | Open a novel  | pyxis://novels/?id=12345        |
    // | Open a user   | pyxis://users/?id=12345         |
    // +---------------+---------------------------------+
    public static class PyxisSchemeActivator
    {
        public static void Activate(Uri uri, INavigationService navigationService)
        {
            var host = uri.Host;
            var activateParams = UrlParameter.ParseQuery(uri.ToString());
            var param = new DetailByIdParameter { Id = activateParams["id"] }.ToJson();
            switch (host)
            {
                case "illusts":
                    navigationService.Navigate("Detail.IllustDetail", param);
                    break;

                case "novels":
                    navigationService.Navigate("Detail.NovelDetail", param);
                    break;

                case "users":
                    navigationService.Navigate("Detail.UserDetail", param);
                    break;
            }
        }
    }
}