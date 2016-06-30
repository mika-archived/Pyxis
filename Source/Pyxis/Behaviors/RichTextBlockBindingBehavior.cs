using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    // 単純に Binding 可能にするだけ。
    internal class RichTextBlockBindingBehavior : Behavior<RichTextBlock>
    {
        public static readonly DependencyProperty BindingDocumentProperty =
            DependencyProperty.Register(nameof(BindingDocument), typeof(List<Block>),
                                        typeof(RichTextBlockBindingBehavior),
                                        new PropertyMetadata(null, PropertyChangedCallback));

        public List<Block> BindingDocument
        {
            get { return (List<Block>) GetValue(BindingDocumentProperty); }
            set { SetValue(BindingDocumentProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBlock = (sender as RichTextBlockBindingBehavior)?.AssociatedObject;
            textBlock?.Blocks.Clear();

            var blockCollection = e.NewValue as List<Block>;
            if (blockCollection == null)
                return;
            foreach (var block in blockCollection)
                textBlock?.Blocks.Add(block);
        }
    }
}