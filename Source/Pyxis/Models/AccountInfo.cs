using Sagitta.Models;

namespace Pyxis.Models
{
    public class AccountInfo
    {
        public string Username { get; }

        public string Password { get; }

        public Me Account { get; }

        public AccountInfo(string username, string password, Me account = null)
        {
            Username = username;
            Password = password;
            Account = account;
        }
    }
}