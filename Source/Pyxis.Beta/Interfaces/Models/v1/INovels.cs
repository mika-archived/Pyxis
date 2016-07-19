﻿using System.Collections.Generic;

using Pyxis.Beta.Interfaces.Models.Internal;

namespace Pyxis.Beta.Interfaces.Models.v1
{
    public interface INovels : IErrorResponse, IIndex
    {
        IList<INovel> NovelList { get; }
    }
}