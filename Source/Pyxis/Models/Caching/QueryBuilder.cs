using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Pyxis.Models.Caching
{
    public class QueryBuilder
    {
        private readonly List<Expression<Func<string, object>>> _expressions;
        private readonly string _path;

        public QueryBuilder(string path)
        {
            _expressions = new List<Expression<Func<string, object>>>();
            _path = path;
        }

        public void AddParams(params Expression<Func<string, object>>[] parameters)
        {
            foreach (var expression in parameters)
                _expressions.Add(expression);
        }

        public string ToQuery()
        {
            var kvps = _expressions.Select(w => new KeyValuePair<string, string>(w.Parameters[0].Name, w.Compile().Invoke(null).ToString()));
            var _params = kvps.Where(w => !string.IsNullOrWhiteSpace(w.Value)).Select(w => $"{w.Key}={Uri.EscapeDataString(w.Value)}");
            return $"{_path}?{string.Join("&", _params)}";
        }
    }
}