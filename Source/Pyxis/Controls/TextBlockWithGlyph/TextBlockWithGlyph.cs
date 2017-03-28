using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    public sealed class TextBlockWithGlyph : Control
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextBlockWithGlyph), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextStyleProperty =
            DependencyProperty.Register(nameof(TextStyle), typeof(Style), typeof(TextBlockWithGlyph), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(TextBlockWithGlyph), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty GlyphStyleProperty =
            DependencyProperty.Register(nameof(GlyphStyle), typeof(Style), typeof(TextBlockWithGlyph), new PropertyMetadata(default(Style)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Style TextStyle
        {
            get { return (Style) GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public string Glyph
        {
            get { return (string) GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public Style GlyphStyle
        {
            get { return (Style) GetValue(GlyphStyleProperty); }
            set { SetValue(GlyphStyleProperty, value); }
        }

        public TextBlockWithGlyph()
        {
            DefaultStyleKey = typeof(TextBlockWithGlyph);
        }
    }
}