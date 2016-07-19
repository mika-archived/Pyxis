using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

using Pyxis.Models;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly List<Category> _categoryTable = new List<Category>
        {
            new Category("Home", "ホーム", 0),
            new Category("New", "新着", 1),
            new Category("Search", "検索", 2),
            new Category("Work", "作品", 4),
            new Category("Favorite", "お気に入り", 5),
            new Category("BrowsingHistory", "閲覧履歴", 6),
            new Category("Bookmark", "ブックマーク", 7),
            new Category("Following", "フォロー", 9),
            new Category("Follower", "フォロワー", 10),
            new Category("Mypixiv", "マイピク", 11),
            new Category("Settings", "設定", 13),
            new Category("Detail", "詳細", -1),
            new Category("Ranking.Illust", "イラストランキング", -1),
            new Category("Ranking.Manga", "漫画ランキング", -1),
            new Category("Ranking.Novel", "小説ランキング", -1)
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
            {
                foreach (var kvp in _categoryTable)
                {
                    if (!classNameWithNamespace.Contains(kvp.Key))
                        continue;
                    index = kvp.Index;
                    name = kvp.Name;
                }
            }
            Index = index;
            Name = name;
            UpdateRequired = true;
        }

        #endregion
    }
}