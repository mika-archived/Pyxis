using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Windows.Security.Credentials;

using Pyxis.Constants;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Services
{
    public class AccountService : IAccountService
    {
        private readonly PixivClient _pixivClient;

        public AccountService(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;
        }

        public async Task<bool> LoginAsync()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();
                var credentials = vault.FindAllByResource(PyxisConstants.ResourceId);
                var credential = credentials.FirstOrDefault(w => !w.UserName.EndsWith("$deviceToken"));
                if (credential == null)
                    return false;
                credential.RetrievePassword();
                var deviceToken = credentials.FirstOrDefault(w => w.UserName == $"{credential.UserName}$deviceToken");
                deviceToken?.RetrievePassword();

                var tokens = await _pixivClient.Authentication.LoginAsync(credential.UserName, credential.Password, deviceToken?.Password);
                vault.Add(new PasswordCredential(PyxisConstants.ResourceId, $"{credential.UserName}$deviceToken", tokens.DeviceToken));

                if (tokens.User != null)
                {
                    CurrentUser = tokens.User;
                }
                else
                {
                    await LogoutAsync();
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                await LogoutAsync();
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var tokens = await _pixivClient.Authentication.LoginAsync(username, password);
                if (tokens == null)
                    return false;

                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(PyxisConstants.ResourceId, username, password));
                vault.Add(new PasswordCredential(PyxisConstants.ResourceId, $"{username}$deviceToken", tokens.DeviceToken));

                CurrentUser = tokens.User;
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return false;
        }

        public Task<bool> LogoutAsync()
        {
            try
            {
                var vault = new PasswordVault();
                var credentials = vault.FindAllByResource(PyxisConstants.ResourceId);
                foreach (var credential in credentials)
                    vault.Remove(credential);

                CurrentUser = null;
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return Task.FromResult(false);
        }

        public event EventHandler<User> CurrentUserCganged;

        #region CurrentUser

        private User _currentUser;

        public User CurrentUser
        {
            get => _currentUser;
            private set
            {
                if (_currentUser?.Id == value?.Id)
                    return;
                _currentUser = value;
                CurrentUserCganged?.Invoke(this, value);
            }
        }

        #endregion
    }
}