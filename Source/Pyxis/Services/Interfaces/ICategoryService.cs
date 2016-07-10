using System.Runtime.CompilerServices;

namespace Pyxis.Services.Interfaces
{
    public interface ICategoryService
    {
        string Name { get; }

        int Index { get; }

        void UpdateCategory([CallerFilePath] string filePath = "");
    }
}