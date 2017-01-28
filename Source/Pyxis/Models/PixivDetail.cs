using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;
using Pyxis.Services.Interfaces;

namespace Pyxis.Models
{
    internal class PixivDetail : BindableBase
    {
        private readonly string _id;
        private readonly IPixivClient _pixivClient;
        private readonly IQueryCacheService _queryCacheService;
        private readonly SearchType _searchType;

        public PixivDetail(string id, SearchType searchType, IPixivClient pixivClient, IQueryCacheService queryCacheService)
        {
            _id = id;
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
            IllustDetail = await _queryCacheService.RunAsync(_pixivClient.IllustV1.DetailAsync, illust_id => _id);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchUser()
        {
            UserDetail = await _queryCacheService.RunAsync(_pixivClient.UserV1.DetailAsync, user_id => _id, filter => "for_ios");
        }

        #region IllustDetail

        private IIllust _illustDetail;

        public IIllust IllustDetail
        {
            get { return _illustDetail; }
            set { SetProperty(ref _illustDetail, value); }
        }

        #endregion

        #region NovelDetail

        private INovel _novelDetail;

        public INovel NovelDetail
        {
            get { return _novelDetail; }
            set { SetProperty(ref _novelDetail, value); }
        }

        #endregion

        #region UserDetail

        private IUserDetail _userDetail;

        public IUserDetail UserDetail
        {
            get { return _userDetail; }
            set { SetProperty(ref _userDetail, value); }
        }

        #endregion
    }
}