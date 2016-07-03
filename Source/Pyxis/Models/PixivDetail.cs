using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Models.Enums;

namespace Pyxis.Models
{
    internal class PixivDetail : BindableBase
    {
        private readonly string _id;
        private readonly IPixivClient _pixivClient;
        private readonly SearchType _searchType;

        public PixivDetail(string id, SearchType searchType, IPixivClient pixivClient)
        {
            _id = id;
            _searchType = searchType;
            _pixivClient = pixivClient;
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
            IllustDetail = await _pixivClient.IllustV1.DetailAsync(illust_id => _id);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task FetchUser()
        {
            UserDetail = await _pixivClient.User.DetailAsync(user_id => _id, filter => "for_ios");
        }

        #region IllustDetail

        private IIllust _illustDetail;

        public IIllust IllustDetail
        {
            get { return _illustDetail; }
            set { SetProperty(ref _illustDetail, value); }
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