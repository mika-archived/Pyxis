using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

using Windows.Data.Html;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

using HtmlAgilityPack;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;

namespace Pyxis.Converters
{
    // string から List<Block> を作成する。
    // https://code.msdn.microsoft.com/Social-Media-Dashboard-135436da
    internal class HtmlStringToBlockCollectionConverter : DependencyObject, IValueConverter
    {
        private readonly Regex _colorCode = new Regex("#[0-9A-Fa-f]{3,6}", RegexOptions.Compiled);
        private INavigationService _navigationService;

        private List<Block> GenerateBlockContents(string html)
        {
            var blocks = new List<Block>();
            try
            {
                var document = new HtmlDocument();
                document.LoadHtml(html);
                blocks.Add(GenerateParagraph(document.DocumentNode));
            }
            catch
            {
                // ignored
            }
            return blocks;
        }

        private Paragraph GenerateParagraph(HtmlNode node)
        {
            var paragraph = new Paragraph();
            AddChildren(paragraph, node);
            return paragraph;
        }

        private void AddChildren(Paragraph paragraph, HtmlNode node)
        {
            var added = false;
            foreach (var childNode in node.ChildNodes)
            {
                var inline = GenerateBlocksForNode(childNode);
                if (inline == null)
                    continue;
                paragraph.Inlines.Add(inline);
                added = true;
            }
            if (!added)
                paragraph.Inlines.Add(new Run {Text = GeneratePlainText(node)});
        }

        private void AddChildren(Span span, HtmlNode node)
        {
            var added = false;
            foreach (var childNode in node.ChildNodes)
            {
                var inline = GenerateBlocksForNode(childNode);
                if (inline == null)
                    continue;
                span.Inlines.Add(inline);
                added = true;
            }
            if (!added)
                span.Inlines.Add(new Run {Text = GeneratePlainText(node)});
        }

        private Inline GenerateBlocksForNode(HtmlNode node)
        {
            switch (node.Name)
            {
                case "a":
                    return GenerateHyperlink(node);

                case "br":
                    return new LineBreak();

                case "em":
                    return GenerateEmphasis(node);

                // なくない？
                case "s":
                    return new Run {Text = GeneratePlainText(node)};

                case "p":
                    return GenerateInnerParagraph(node);

                case "span":
                    return GenerateSpan(node);

                case "strong":
                    return GenerateStrong(node);

                case "#text":
                    if (!string.IsNullOrWhiteSpace(node.InnerText))
                        return new Run {Text = GeneratePlainText(node)};
                    break;

                default:
                    return GenerateSpawnNewLine(node);
            }
            return null;
        }

        private string GeneratePlainText(HtmlNode node) => HtmlUtilities.ConvertToText(node.InnerText);

        private Inline GenerateHyperlink(HtmlNode node)
        {
            Hyperlink hyperlink;
            if (node.Attributes["href"].Value.StartsWith("pixiv://"))
            {
                var uri = node.Attributes["href"].Value;
                hyperlink = new Hyperlink();
                hyperlink.Click += (sender, args) =>
                {
                    if (uri.StartsWith("pixiv://illusts"))
                    {
                        var parameter = new DetailByIdParameter {Id = uri.Replace("pixiv://illusts/", "")};
                        _navigationService.Navigate("Detail.IllustDetail", parameter.ToJson());
                    }
                };
            }
            else
                hyperlink = new Hyperlink {NavigateUri = new Uri(node.Attributes["href"].Value)};
            hyperlink.Inlines.Add(new Run {Text = GeneratePlainText(node)});
            return hyperlink;
        }

        private Inline GenerateEmphasis(HtmlNode node)
        {
            var italic = new Span {FontStyle = FontStyle.Italic};
            AddChildren(italic, node);
            return italic;
        }

        private Inline GenerateInnerParagraph(HtmlNode node)
        {
            var span = new Span();
            AddChildren(span, node);
            return span;
        }

        private Inline GenerateSpan(HtmlNode node)
        {
            var span = new Span();
            var style = node.Attributes["style"].Value;
            if (_colorCode.IsMatch(style))
            {
                var value = _colorCode.Match(style).Value.Substring(1);
                var r = byte.Parse(value.Substring(1, 2), NumberStyles.HexNumber);
                var g = byte.Parse(value.Substring(3, 2), NumberStyles.HexNumber);
                var b = byte.Parse(value.Substring(5, 2), NumberStyles.HexNumber);
                span.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, r, g, b));
            }
            AddChildren(span, node);
            return span;
        }

        private Inline GenerateStrong(HtmlNode node)
        {
            var bold = new Bold();
            AddChildren(bold, node);
            return bold;
        }

        private Inline GenerateSpawnNewLine(HtmlNode node)
        {
            var span = new Span();
            AddChildren(span, node);
            if (span.Inlines.Count > 0)
                span.Inlines.Add(new LineBreak());
            return span;
        }

        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var values = value as List<object>;
            if (values == null)
                return null;
            var html = values[0] as string;
            _navigationService = values[1] as INavigationService;
            return GenerateBlockContents($"<p>{html}</p>");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}