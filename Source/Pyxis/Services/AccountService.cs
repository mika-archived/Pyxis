using System;
using System.Diagnostics;
using System.Linq;

using Windows.Security.Credentials;

using Pyxis.Models;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    public class AccountService : IAccountService
    {
        public AccountService()
        {
            IsLoggedIn = false;
            IsPremium = false;
        }

        #region Implementation of IAccountService

        public bool IsLoggedIn { get; private set; }
        public bool IsPremium { get; private set; }

        public void Clear()
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
        }

        public void Save(AccountInfo account)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Add(new PasswordCredential(PyxisConstants.ApplicationKey, account.Username, account.Password));

                IsLoggedIn = true;
                IsPremium = account.Account.IsPremium;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public AccountInfo Load()
        {
            try
            {
                var vault = new PasswordVault();
                vault.RetrieveAll();

                var resources = vault.FindAllByResource(PyxisConstants.ApplicationKey);
                var credential = resources.First();
                credential.RetrievePassword();
                return new AccountInfo(credential.UserName, credential.Password);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        #endregion
    }
}