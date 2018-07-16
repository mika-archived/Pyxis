using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace Pyxis.Bindings
{
    [ContentProperty(Name = "Template")]
    public class TypeToTemplate : DependencyObject
    {
        public DataTemplate Template { get; set; }
        public string DataType { get; set; }
        public Type TargeType => DataType != null ? Type.GetType(DataType) : null;
    }
}