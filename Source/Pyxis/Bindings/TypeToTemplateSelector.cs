using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Pyxis.Bindings
{
    [ContentProperty(Name = "Templates")]
    public class TypeToTemplateSelector : DataTemplateSelector
    {
        public TypeToTemplateCollection Templates { get; set; } = new TypeToTemplateCollection();

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
                return default(DataTemplate);

            var template = Templates.Where(w => w.TargeType != null && item.GetType() == w.TargeType)
                                    .Select(w => w.Template)
                                    .FirstOrDefault();
            return template ?? base.SelectTemplateCore(item, container);
        }
    }
}