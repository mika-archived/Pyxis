using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ParamExpr = System.Linq.Expressions.Expression<System.Func<string, object>>;

namespace Pyxis.Alpha
{
    internal static class ParameterUtil
    {
        internal static Func<ParamExpr, string> Name => expr => expr.Parameters[0].Name;

        internal static Func<ParamExpr, string> Value => expr => expr.Compile().Invoke(null).ToString();

        internal static ParamExpr[] Merge(string name, ParamExpr[] parameters)
        {
            var target = parameters.First(w => Name(w) == name);
            var others = parameters.Where(w => Name(w) != name);
            var fixedParams = new List<ParamExpr>();
            foreach (var value in Value(target).Split(',').Select((w, i) => new {Index = i, Value = w}))
            {
                var expr = Expression.Lambda<Func<string, object>>(Expression.Constant(value.Value),
                                                                   Expression.Parameter(typeof(string), $"{name}[{value.Index}]"));
                fixedParams.Add(expr);
            }
            fixedParams.AddRange(others);
            return fixedParams.ToArray();
        }
    }
}