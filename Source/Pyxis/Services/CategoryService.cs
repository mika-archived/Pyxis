using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using Windows.ApplicationModel.Resources;

using Pyxis.Models;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class CategoryService : ICategoryService
    {
        private static readonly ResourceLoader Resources = ResourceLoader.GetForCurrentView();

        private readonly List<Category> _categoryTable = new List<Category>
        {
            new Category("Home", Resources.GetString("Home/Text"), 0),
            new Category("New", Resources.GetString("New/Text"), 1),
            new Category("Search", Resources.GetString("Search/Text"), 2),
            new Category("Work", Resources.GetString("Work/Text"), 4),
            new Category("Favorite", Resources.GetString("Favorite/Text"), 5),
            new Category("BrowsingHistory", Resources.GetString("BrowsingHistory/Text"), 6),
            new Category("Bookmark", Resources.GetString("Bookmark/Text"), 7),
            new Category("Following", Resources.GetString("Following/Text"), 9),
            new Category("Follower", Resources.GetString("Follower/Text"), 10),
            new Category("Mypixiv", Resources.GetString("Mypixiv/Text"), 11),
            new Category("BlockedUsers", Resources.GetString("BlockedUsers/Text"), 12),
            new Category("Settings", Resources.GetString("Settings/Text"), 14),
            new Category("Detail", Resources.GetString("Detail/Text"), -1),
            new Category("Ranking.Illust", Resources.GetString("IllustRanking/Text"), -1),
            new Category("Ranking.Manga", Resources.GetString("MangaRanking/Text"), -1),
            new Category("Ranking.Novel", Resources.GetString("NovelRanking/Text"), -1)
        };

        public CategoryService()
        {
            Index = -1;
        }

        #region Implementation of ICategoryService

        #region Name

        private string _name;

        public string Name
        {
            get
            {
                UpdateRequired = false;
                return _name;
            }
            private set { _name = value; }
        }

        #endregion

        public int Index { get; private set; }
        public bool UpdateRequired { get; private set; }

        public void UpdateCategory([CallerFilePath] string filePath = "")
        {
            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            var firstIndex = filePath.IndexOf("Source\\Pyxis");
            var fullName = Path.GetFileNameWithoutExtension(filePath.Substring(firstIndex + "Source\\".Length)
                                                                    .Replace("\\", "."));
            var classNameWithNamespace = fullName.Replace(typeof(App).Namespace + ".ViewModels.", "");
            var index = -1;
            var name = "";
            foreach (var kvp in _categoryTable)
            {
                if (!classNameWithNamespace.StartsWith(kvp.Key))
                    continue;
                index = kvp.Index;
                name = kvp.Name;
            }
            if (index == -1)
                foreach (var kvp in _categoryTable)
                {
                    if (!classNameWithNamespace.Contains(kvp.Key))
                        continue;
                    index = kvp.Index;
                    name = kvp.Name;
                }
            Index = index;
            Name = name;
            UpdateRequired = true;
        }

        #endregion
    }
}