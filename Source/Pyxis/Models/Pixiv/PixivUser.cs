using System.Threading.Tasks;

using Sagitta;
using Sagitta.Enum;
using Sagitta.Models;

namespace Pyxis.Models.Pixiv
{
    /// <summary>
    ///     ユーザー詳細
    /// </summary>
    internal class PixivUser : PixivModel
    {
        public PixivUser(PixivClient pixivClient) : base(pixivClient) {}

        public async Task FetchAsync(int userId)
        {
            UserDetail = await PixivClient.User.DetailAsync(userId);
            Illusts = await PixivClient.User.IllustsAsync(IllustType.Illust, userId);
            Mangas = await PixivClient.User.IllustsAsync(IllustType.Manga, userId);
            Novels = await PixivClient.User.NovelsAsync(userId);
            BookmarkIllusts = await PixivClient.User.Bookmarks.IllustAsync(userId);
            BookmarkNovels = await PixivClient.User.Bookmarks.NovelAsync(userId);
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

        private IllustCollection _illusts;

        public IllustCollection Illusts
        {
            get { return _illusts; }
            set { SetProperty(ref _illusts, value); }
        }

        #endregion

        #region Mangas

        private IllustCollection _mangas;

        public IllustCollection Mangas
        {
            get { return _mangas; }
            set { SetProperty(ref _mangas, value); }
        }

        #endregion

        #region Novels

        private NovelCollection _novels;

        public NovelCollection Novels
        {
            get { return _novels; }
            set { SetProperty(ref _novels, value); }
        }

        #endregion

        #region BookmarkIllusts

        private IllustCollection _bookmarkIllusts;

        public IllustCollection BookmarkIllusts
        {
            get { return _bookmarkIllusts; }
            set { SetProperty(ref _bookmarkIllusts, value); }
        }

        #endregion

        #region BookmarkNovels

        private NovelCollection _bookmarkNovels;

        public NovelCollection BookmarkNovels
        {
            get { return _bookmarkNovels; }
            set { SetProperty(ref _bookmarkNovels, value); }
        }

        #endregion
    }
}