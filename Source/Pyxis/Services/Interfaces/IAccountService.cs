using System.Threading.Tasks;

using Pyxis.Models;

using Sagitta.Models;

namespace Pyxis.Services.Interfaces
{
    public interface IAccountService
    {
        bool IsLoggedIn { get; }

        bool IsPremium { get; }

        Me LoggedInAccount { get; }

        void Clear();

        void Save(AccountInfo account);

        Task Login();
    }
}