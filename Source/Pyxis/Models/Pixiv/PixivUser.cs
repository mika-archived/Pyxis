using System.Threading.Tasks;

using Prism.Mvvm;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    /// <summary>
    ///     ユーザー詳細
    /// </summary>
    internal class PixivUser : BindableBase
    {
        private readonly PixivClient _pixivClient;

        public PixivUser(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
        }

        public async Task FetchAsync(int userId)
        {
            UserDetail = await _pixivClient.User.DetailAsync(userId);
            Illusts = await _pixivClient.User.IllustsAsync(IllustType.Illust, userId);
            Mangas = await _pixivClient.User.IllustsAsync(IllustType.Manga, userId);
            Novels = await _pixivClient.User.NovelsAsync(userId);
            BookmarkIllusts = await _pixivClient.User.Bookmarks.IllustAsync(userId);
            BookmarkNovels = await _pixivClient.User.Bookmarks.NovelAsync(userId);
        }

        #region User

        private UserDetail _userDetail;

        public UserDetail UserDetail
        {
            get { return _userDetail; }
            set { SetProperty(ref _userDetail, value); }
        }

        #endregion

        #region Illusts

        private IllustsRoot _illusts;

        public IllustsRoot Illusts
        {
            get { return _illusts; }
            set { SetProperty(ref _illusts, value); }
        }

        #endregion

        #region Mangas

        private IllustsRoot _mangas;

        public IllustsRoot Mangas
        {
            get { return _mangas; }
            set { SetProperty(ref _mangas, value); }
        }

        #endregion

        #region Novels

        private NovelsRoot _novels;

        public NovelsRoot Novels
        {
            get { return _novels; }
            set { SetProperty(ref _novels, value); }
        }

        #endregion

        #region BookmarkIllusts

        private IllustsRoot _bookmarkIllusts;

        public IllustsRoot BookmarkIllusts
        {
            get { return _bookmarkIllusts; }
            set { SetProperty(ref _bookmarkIllusts, value); }
        }

        #endregion

        #region BookmarkNovels

        private NovelsRoot _bookmarkNovels;

        public NovelsRoot BookmarkNovels
        {
            get { return _bookmarkNovels; }
            set { SetProperty(ref _bookmarkNovels, value); }
        }

        #endregion
    }
}