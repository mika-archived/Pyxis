using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Models
{
    public class AccountInfo
    {
        public string Username { get; }

        public string Password { get; }

        public string DeviceId { get; }

        public IAccount Account { get; }

        public AccountInfo(string username, string password, string deviceId, IAccount account = null)
        {
            Username = username;
            Password = password;
            DeviceId = deviceId;
            Account = account;
        }
    }
}