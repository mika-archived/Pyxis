using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Microsoft.Xaml.Interactivity;

using Pyxis.Attach;

namespace Pyxis.Behaviors
{
    internal sealed class AttachNavigationToPivotBehavior : Behavior<Pivot>
    {
        public static readonly DependencyProperty RootFrameProperty =
            DependencyProperty.Register(nameof(RootFrame), typeof(Frame), typeof(AttachNavigationToPivotBehavior),
                                        new PropertyMetadata(null));

        private readonly Stack<int> _pageStack;

        private bool _isAttached;
        private int _oldIndex; // 1つ前の Index

        public Frame RootFrame
        {
            get { return (Frame) GetValue(RootFrameProperty); }
            set { SetValue(RootFrameProperty, value); }
        }

        public AttachNavigationToPivotBehavior()
        {
            _pageStack = new Stack<int>();
            _isAttached = false;
            _oldIndex = -1;
        }

        // https://github.com/PrismLibrary/Prism/blob/3dded2/Source/Windows10/Prism.Windows/PrismApplication.cs#L148-L171
        private Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = GetType().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(GetType().FullName,
                                                                         typeof(App).Namespace + ".Views.{0}Page");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
                throw new ArgumentException(string.Format("{0}'{1}' is not found.", nameof(pageToken), pageToken));

            return viewType;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            if (!_isAttached)
            {
                RootFrame.Navigating += RootFrameOnNavigating;
                _isAttached = true;
            }
            ReplaceRootFrame();
        }

        private void ReplaceRootFrame()
        {
            var item = AssociatedObject.ItemsPanelRoot?.Children[AssociatedObject.SelectedIndex] as PivotItem;
            if (item == null)
                return;
            foreach (var pivot in AssociatedObject.ItemsPanelRoot?.Children.Select((w, i) => new {Item = w, Index = i}))
            {
                if (pivot.Index == AssociatedObject.SelectedIndex)
                    continue;
                ((PivotItem) pivot.Item).Content = new Frame();
            }
            var pageToken = NavigateTo.GetPageToken(item);
            if (!string.IsNullOrWhiteSpace(pageToken))
                RootFrame.Navigate(GetPageType(pageToken));
            item.Content = RootFrame;
        }

        private void RootFrameOnNavigating(object sender, NavigatingCancelEventArgs args)
        {
            if (args.NavigationMode == NavigationMode.Back)
                AssociatedObject.SelectedIndex = _pageStack.Pop();
            else if (args.NavigationMode == NavigationMode.New)
            {
                if (_oldIndex >= 0)
                    _pageStack.Push(_oldIndex);
            }

            _oldIndex = AssociatedObject.SelectedIndex;
            ReplaceRootFrame();
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
        }

        protected override void OnDetaching()
        {
            RootFrame.Navigating -= RootFrameOnNavigating;
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            base.OnDetaching();
        }

        #endregion
    }
}