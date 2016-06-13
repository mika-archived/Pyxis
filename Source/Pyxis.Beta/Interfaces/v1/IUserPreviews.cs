﻿using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Internal;

namespace Pyxis.Beta.Interfaces.v1
{
    public interface IUserPreviews : IIndex
    {
        IList<IUserPreview> UserPreviews { get; }
    }
}