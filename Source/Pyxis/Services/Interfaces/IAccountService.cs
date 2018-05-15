using System;
using System.Threading.Tasks;

using Sagitta.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IAccountService
    {
        User CurrentUser { get; }

        Task<bool> LoginAsync();

        Task<bool> LoginAsync(string username, string password);

        Task<bool> LogoutAsync();

        event EventHandler<User> CurrentUserCganged;
    }
}