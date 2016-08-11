using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Xaml.Interactivity;

using Pyxis.Attach;
using Pyxis.Models;

namespace Pyxis.Behaviors
{
    internal class GridViewEuqalitySizeBehavior : Behavior<ItemsWrapGrid>
    {
        private readonly Regex _parseRegex = new Regex(@"\((?<mww>[0-9]+),(?<size>[0-9]+)\)", RegexOptions.Compiled);

        private int _width;

        public static DependencyProperty IsEnabledHeightProperty =>
            DependencyProperty.Register(nameof(IsEnabledHeight), typeof(bool), typeof(GridViewEuqalitySizeBehavior),
                                        new PropertyMetadata(true));

        public bool IsEnabledHeight
        {
            get { return (bool) GetValue(IsEnabledHeightProperty); }
            set { SetValue(IsEnabledHeightProperty, value); }
        }

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateSize(e.Size);
        }

        private void UpdateSize(Size size)
        {
            var width = size.Width;
            var adaptiveSizes = ParseToAdaptiveSize(AssumSize.GetAssumSize(AssociatedObject));
            foreach (var adaptiveSize in adaptiveSizes)
            {
                if (!(adaptiveSize.MinWindowWidth <= width))
                    continue;
                _width = adaptiveSize.Size;
                break;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var size = e.NewSize;

            var maxColumn = Math.Floor(size.Width / _width);
            var adjustedSize = _width + size.Width % _width / maxColumn;

            if (IsEnabledHeight)
                AssociatedObject.ItemHeight = adjustedSize;
            AssociatedObject.ItemWidth = adjustedSize;
        }

        private List<AdaptiveSize> ParseToAdaptiveSize(string str)
        {
            var list = new List<AdaptiveSize>();
            foreach (Match match in _parseRegex.Matches(str))
                list.Add(new AdaptiveSize(int.Parse(match.Groups["mww"].Value), int.Parse(match.Groups["size"].Value)));
            return list.OrderByDescending(w => w.MinWindowWidth).ToList();
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            Window.Current.SizeChanged += OnWindowSizeChanged;
            AssociatedObject.SizeChanged += OnSizeChanged;
            UpdateSize(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height));
        }

        protected override void OnDetaching()
        {
            Window.Current.SizeChanged -= OnWindowSizeChanged;
            AssociatedObject.SizeChanged -= OnSizeChanged;
            base.OnDetaching();
        }

        #endregion
    }
}