using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Models
{
    internal class PixivDetail : BindableBase
    {
        private readonly int _id;
        private readonly PixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly SearchType _searchType;

        public PixivDetail(string id, SearchType searchType, PixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _id = int.Parse(id);
            _searchType = searchType;
            _pixivClient = pixivClient;
            _queryCacheService = queryCacheService;
        }

        public void Fetch() => RunHelper.RunAsync(FetchAsync);

        private async Task FetchAsync()
        {
            if (_searchType == SearchType.IllustsAndManga)
                await FetchIllust();
            else if (_searchType == SearchType.Users)
                await FetchUser();
            else
                throw new NotSupportedException();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchIllust()
        {
            IllustDetail = await _pixivClient.Illust.DetailAsync(_id);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchUser()
        {
            UserDetail = await _pixivClient.User.DetailAsync(_id);
        }

        #region IllustDetail

        private Illust _illustDetail;

        public Illust IllustDetail
        {
            get { return _illustDetail; }
            set { SetProperty(ref _illustDetail, value); }
        }

        #endregion

        #region NovelDetail

        private Novel _novelDetail;

        public Novel NovelDetail
        {
            get { return _novelDetail; }
            set { SetProperty(ref _novelDetail, value); }
        }

        #endregion

        #region UserDetail

        private UserDetail _userDetail;

        public UserDetail UserDetail
        {
            get { return _userDetail; }
            set { SetProperty(ref _userDetail, value); }
        }

        #endregion
    }
}