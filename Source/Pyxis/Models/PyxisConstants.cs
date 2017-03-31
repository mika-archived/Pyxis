using System;
using System.Collections.Generic;

namespace Pyxis.Models
{
    public static class PyxisConstants
    {
        public static string ApplicationKey => "8B2D7393-617C-4836-AA0E-68B502D9B1C9";
        public static string Branch => "Twilight";
        public static string PlaceholderSquare => "ms-appx:///Assets/Placeholders/Square-200.png";
        public static string PlaceholderBanner => "ms-appx:///Assets/Placeholders/Banner-851.png";

        public static Lazy<List<Software>> Softwares => new Lazy<List<Software>>(() => new List<Software>
        {
            new Software {Name = "Entity Framework Core", Author = "Microsoft", Url = "https://github.com/aspnet/EntityFramework"},
            new Software {Name = "HockeySDK.UWP", Author = "Microsoft", Url = "https://github.com/bitstadium/HockeySDK-Windows"},
            new Software {Name = "Html Agility Pack", Author = "Simon Mourrier, Jeff Klawiter, Stephan Grell", Url = "http://htmlagilitypack.codeplex.com/"},
            new Software {Name = "Microsoft.Data.Sqlite", Author = ".NET Foundation", Url = "https://github.com/aspnet/Microsoft.Data.Sqlite"},
            new Software {Name = "Newtonsoft.Json", Author = "James Newton-King", Url = "http://www.newtonsoft.com/json"},
            new Software {Name = "Prism", Author = "Brian Lagunas, Brian Noyes", Url = "https://github.com/PrismLibrary/Prism"},
            new Software {Name = "Reactive Extensions", Author = ".NET Foundation and Contributors", Url = "https://github.com/Reactive-Extensions/Rx.NET"},
            new Software {Name = "ReactiveProperty", Author = "neuecc xin9le okazuki", Url = "https://github.com/runceel/ReactiveProperty"},
            new Software {Name = "UWP Community Toolkit", Author = "Microsoft", Url = "https://github.com/Microsoft/UWPCommunityToolkit"},
            new Software
            {
                Name = "WinRT XAML Toolkit for Windows 10",
                Author = "Filip Skakun, Thomas Martinsen, Mahmoud Moussa, Joost van Schaik",
                Url = "https://github.com/xyzzer/WinRTXamlToolkit"
            }
        });
    }
}