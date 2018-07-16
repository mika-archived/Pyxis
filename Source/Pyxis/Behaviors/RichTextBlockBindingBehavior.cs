using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    internal class RichTextBlockBindingBehavior : Behavior<RichTextBlock>
    {
        public static readonly DependencyProperty BindingDocumentProperty =
            DependencyProperty.Register(nameof(BindingDocument), typeof(List<Block>), typeof(RichTextBlockBindingBehavior), new PropertyMetadata(null, PropertyChangedCallback));

        public List<Block> BindingDocument
        {
            get => (List<Block>) GetValue(BindingDocumentProperty);
            set => SetValue(BindingDocumentProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = (sender as RichTextBlockBindingBehavior)?.AssociatedObject;
            textBlock?.Blocks.Clear();

            if (!(e.NewValue is List<Block> blockCollection))
                return;
            foreach (var block in blockCollection)
                textBlock?.Blocks.Add(block);
        }
    }
}