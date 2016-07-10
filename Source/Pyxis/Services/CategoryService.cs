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
            new Category("Home", "ホーム", 1),
            new Category("New", "新着", 2),
            new Category("Search", "検索", 3),
            new Category("Work", "作品", 5),
            new Category("Favorite", "お気に入り", 6),
            new Category("BrowsingHistory", "閲覧履歴", 7),
            new Category("Bookmark", "ブックマーク", 8),
            new Category("Following", "フォロー", 10),
            new Category("Follower", "フォロワー", 11),
            new Category("Mypixiv", "マイピク", 12),
            new Category("Settings", "設定", 14),
            new Category("Detail", "詳細", -1)
        };

        public CategoryService()
        {
            Index = 0;
        }

        #region Implementation of ICategoryService

        public string Name { get; private set; }
        public int Index { get; private set; }

        public void UpdateCategory([CallerFilePath] string filePath = "")
        {
            // ReSharper disable once StringIndexOfIsCultureSpecific.1
            var firstIndex = filePath.IndexOf("Source\\Pyxis");
            var fullName = Path.GetFileNameWithoutExtension(filePath.Substring(firstIndex + "Source\\".Length)
                                                                    .Replace("\\", "."));
            var classNameWithNamespace = fullName.Replace(typeof(App).Namespace + ".ViewModels", "");
            var index = 0;
            var name = "";
            foreach (var kvp in _categoryTable)
            {
                if (!classNameWithNamespace.StartsWith(kvp.Key))
                    continue;
                index = kvp.Index;
                name = kvp.Name;
            }
            if (index == 0)
            {
                foreach (var kvp in _categoryTable)
                {
                    if (!classNameWithNamespace.Contains(kvp.Key))
                        continue;
                    index = kvp.Index;
                    name = kvp.Name;
                }
            }
            if (index <= 0)
            {
                Index = 0;
                return;
            }

            Index = index;
            Name = name;
        }

        #endregion
    }
}