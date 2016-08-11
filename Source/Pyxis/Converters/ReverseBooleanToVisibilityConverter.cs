using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Pyxis.Converters
{
    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var b = value as bool?;
            if (b.HasValue && b.Value)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}