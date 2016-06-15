using System.Collections.Generic;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface IMetadata
    {
        IZipUrls ZipUrls { get; }

        IList<IFrame> Frames { get; }
    }
}