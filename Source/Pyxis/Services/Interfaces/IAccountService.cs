using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IAccountService
    {
        bool IsLoggedIn { get; }

        bool IsPremium { get; }

        IAccount LoggedInAccount { get; }

        void Clear();

        void Save(AccountInfo account);

        Task Login();
    }
}