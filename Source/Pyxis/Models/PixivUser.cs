using System.Threading.Tasks;

using Prism.Mvvm;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;

namespace Pyxis.Models
{
    internal class PixivUser : BindableBase
    {
        private readonly string _id;
        private readonly IPixivClient _pixivClient;

        public PixivUser(string id, IPixivClient pixivClient)
        {
            _id = id;
            _pixivClient = pixivClient;
            RunHelper.RunAsync(UpdateUser);
        }

        private async Task UpdateUser()
        {
            UserDetail = await _pixivClient.User.DetailAsync(user_id => _id, filter => "for_ios");
        }

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