using System;

using Pyxis.Helpers;

namespace Pyxis.Models.Caching
{
    public class QueryCache
    {
        public string Query { get; set; }

        public object Result { get; set; }

        public bool IsEnabled { get; private set; }

        public QueryCache()
        {
            IsEnabled = true;
            RunHelper.RunLater(() => IsEnabled = false, TimeSpan.FromMinutes(5));
        }
    }
}