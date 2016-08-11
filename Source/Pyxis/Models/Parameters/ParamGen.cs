using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Pyxis.Models.Parameters
{
    internal static class ParamGen
    {
        internal static IEnumerable<string> Generate<T>(T @base, params Expression<Func<T, object>>[] targetObjects)
            where T : ParameterBase
        {
            return GenerateRaw(@base, targetObjects).Select(w => (string) w.ToJson());
        }

        internal static IEnumerable<T> GenerateRaw<T>(T @base, params Expression<Func<T, object>>[] targetObjects)
            where T : ParameterBase
        {
            var list = new List<T>();
            foreach (var targetObject in targetObjects)
            {
                var target = targetObject.Compile().Invoke(@base);
                if (!target.GetType().GetTypeInfo().IsEnum)
                    throw new NotSupportedException();
                foreach (var value in Enum.GetValues(target.GetType()))
                {
                    var obj = (T) @base.Clone();
                    var name = ((MemberExpression) ((UnaryExpression) targetObject.Body).Operand).Member.Name;
                    var info = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                    info.SetValue(obj, value);
                    list.Add(obj);
                }
            }
            return list;
        }
    }
}