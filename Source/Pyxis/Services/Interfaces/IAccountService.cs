using System;
using System.Threading.Tasks;

using Sagitta.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IAccountService
    {
        Me Account { get; }

        Task LoginAsync();

        Task LoginAsync(string username, string password);

        Task LogoutAsync();

        Task ClearAsync();

        event EventHandler OnLoggedIn;

        event EventHandler OnLoggedOut;
    }
}