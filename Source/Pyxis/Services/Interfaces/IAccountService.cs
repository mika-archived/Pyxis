using System.Threading.Tasks;

using Pyxis.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IAccountService
    {
        bool IsLoggedIn { get; }

        bool IsPremium { get; }

        void Clear();

        void Save(AccountInfo account);

        Task Login();
    }
}