using System.Collections.Generic;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Pyxis.Bindings
{
    [ContentProperty(Name = "Templates")]
    public class TypeToTemplateSelector : DataTemplateSelector
    {
        // ReSharper disable once CollectionNeverUpdated.Global
        public List<TypeToTemplate> Templates { get; set; } = new List<TypeToTemplate>();

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null)
                return default(DataTemplate);

            var template = Templates.Where(w => w.TargetType != null && item.GetType() == w.TargetType)
                                    .Select(w => w.Template)
                                    .FirstOrDefault();
            return template ?? base.SelectTemplateCore(item, container);
        }
    }
}