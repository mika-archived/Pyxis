using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Pyxis.Converters
{
    internal class EnumToBooleanConverter : IValueConverter
    {
        private Type GetType(object parameter)
        {
            var paramStr = parameter as string;
            if (paramStr == null)
                throw new ArgumentException(nameof(parameter));
            var typeNameWithNamespace = paramStr.Substring(0, paramStr.LastIndexOf(".", StringComparison.Ordinal));
            return Type.GetType(typeNameWithNamespace);
        }

        private string GetValue(object parameter)
        {
            var paramStr = parameter as string;
            if (paramStr == null)
                throw new ArgumentException(nameof(parameter));
            return paramStr.Substring(paramStr.LastIndexOf(".", StringComparison.Ordinal) + 1);
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value.ToString() == GetValue(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            var b = value as bool?;
            if (!b.HasValue || !b.Value)
                return DependencyProperty.UnsetValue;
            return Enum.Parse(GetType(parameter), GetValue(parameter), true);
        }

        #endregion
    }
}