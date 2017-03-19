using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Windows.Security.Credentials;

using Pyxis.Models;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Services
{
    internal class AccountService : IAccountService
    {
        private readonly PixivClient _pixivClient;

        public AccountService(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
        }

        public async Task LoginAsync()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();

                var resources = vault.FindAllByResource(PyxisConstants.ApplicationKey);
                var credentials = resources.FirstOrDefault(w => !w.UserName.EndsWith("+DeviceId"));
                if (credentials == null)
                    return;
                credentials.RetrievePassword();

                var deviceCredentials = resources.FirstOrDefault(w => w.UserName == $"{credentials.UserName}+DeviceId");
                var deviceId = "pixiv";
                if (deviceCredentials != null)
                {
                    deviceCredentials.RetrievePassword();
                    deviceId = deviceCredentials.Password;
                }

                var oauthToken = await _pixivClient.OAuth.TokenAsync(credentials.UserName, credentials.Password, deviceId);
                if (oauthToken == null)
                {
                    await ClearAsync();
                    return;
                }

                Account = oauthToken.User;
                vault.Add(new PasswordCredential(PyxisConstants.ApplicationKey, $"{Account.Name}+DeviceId", oauthToken.DeviceToken));
                OnLoggedIn?.Invoke(this, new EventArgs());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public Task LogoutAsync()
        {
            Account = null;
            OnLoggedOut?.Invoke(this, new EventArgs());
            return Task.CompletedTask;
        }

        public Task ClearAsync()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();

                var resources = vault.FindAllByResource(PyxisConstants.ApplicationKey);
                foreach (var credential in resources)
                    vault.Remove(credential);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return Task.CompletedTask;
        }

        public Me Account { get; private set; }

        public event EventHandler OnLoggedIn;

        public event EventHandler OnLoggedOut;
    }
}