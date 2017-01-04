using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

using Windows.Security.Credentials;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Models;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPixivClient _pixivClient;

        public AccountService(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            IsLoggedIn = false;
            IsPremium = false;
        }

        #region Implementation of IAccountService

        public bool IsLoggedIn { get; private set; }
        public bool IsPremium { get; private set; }

        public IAccount LoggedInAccount { get; private set; }

        public void Clear()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();

                var resources = vault.FindAllByResource(PyxisConstants.ApplicationKey);
                foreach (var credential in resources)
                    vault.Remove(credential);
                IsLoggedIn = false;
                IsPremium = false;
                LoggedInAccount = null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void Save(AccountInfo account)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(PyxisConstants.ApplicationKey, account.Username, account.Password));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public async Task Login()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();

                var resources = vault.FindAllByResource(PyxisConstants.ApplicationKey);
                var credential = resources.FirstOrDefault(w => !w.UserName.Contains("+DeviceId"));
                if (credential == null)
                    return;
                credential.RetrievePassword();
                var deviceCredential = resources.FirstOrDefault(w => w.UserName == $"{credential.UserName}+DeviceId");
                var deviceId = "pixiv";
                if (deviceCredential != null)
                {
                    // Last login
                    deviceCredential.RetrievePassword();
                    deviceId = deviceCredential.Password;
                }
                var account = await _pixivClient.Authorization
                                                .Login(get_secure_url => 1,
                                                       grant_type => "password",
                                                       client_secret => "HP3RmkgAmEGro0gn1x9ioawQE8WMfvLXDz3ZqxpK",
                                                       device_token => deviceId,
                                                       password => credential.Password,
                                                       client_id => "bYGKuGVw91e0NMfPGp44euvGt59s",
                                                       username => credential.UserName);
                if (account == null)
                {
                    Clear();
                    return;
                }
                IsLoggedIn = true;
                IsPremium = account.User.IsPremium;
                LoggedInAccount = account.User;
                vault.Add(new PasswordCredential(PyxisConstants.ApplicationKey, $"{credential.UserName}+DeviceId", account.DeviceToken));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        #endregion
    }
}