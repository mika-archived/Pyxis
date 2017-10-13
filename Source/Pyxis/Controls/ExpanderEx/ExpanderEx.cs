using Windows.UI.Xaml;

using Microsoft.Toolkit.Uwp.UI.Controls;

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    internal class ExpanderEx : Expander
    {
        public new static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(ExpanderEx), new PropertyMetadata(default(object)));

        public new object Header
        {
            get => GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
    }
}