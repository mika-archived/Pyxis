using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    internal class AdjustControlSizeBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IsSyncHeightProperty =
            DependencyProperty.Register(nameof(IsSyncHeight), typeof(bool),
                                        typeof(AttachNavigateToListBoxBehavior), new PropertyMetadata(false));

        public static readonly DependencyProperty HeightOffsetProperty =
            DependencyProperty.Register(nameof(HeightOffset), typeof(int),
                                        typeof(AttachNavigateToListBoxBehavior), new PropertyMetadata(0));

        public static readonly DependencyProperty IsSyncWidthProperty =
            DependencyProperty.Register(nameof(IsSyncWidth), typeof(bool),
                                        typeof(AttachNavigateToListBoxBehavior), new PropertyMetadata(false));

        public static readonly DependencyProperty WidthOffsetProperty =
            DependencyProperty.Register(nameof(WidthOffset), typeof(int),
                                        typeof(AttachNavigateToListBoxBehavior), new PropertyMetadata(0));

        public bool IsSyncHeight
        {
            get { return (bool) GetValue(IsSyncHeightProperty); }
            set { SetValue(IsSyncHeightProperty, value); }
        }

        public int HeightOffset
        {
            get { return (int) GetValue(HeightOffsetProperty); }
            set { SetValue(HeightOffsetProperty, value); }
        }

        public bool IsSyncWidth
        {
            get { return (bool) GetValue(IsSyncWidthProperty); }
            set { SetValue(IsSyncWidthProperty, value); }
        }

        public int WidthOffset
        {
            get { return (int) GetValue(WidthOffsetProperty); }
            set { SetValue(WidthOffsetProperty, value); }
        }

        private void OnSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            AdjustSize(e.Size);
        }

        private void AdjustSize(Size size)
        {
            var offset = 0;
            if (ApiInformation.IsTypePresent(typeof(StatusBar).ToString()))
                offset = -(int) StatusBar.GetForCurrentView().OccludedRect.Height;
            if (IsSyncHeight)
                AssociatedObject.Height = size.Height + HeightOffset + offset;
            if (IsSyncWidth)
                AssociatedObject.Width = size.Width + WidthOffset;
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            Window.Current.SizeChanged += OnSizeChanged;
            AdjustSize(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height));
        }

        protected override void OnDetaching()
        {
            Window.Current.SizeChanged += OnSizeChanged;
            base.OnDetaching();
        }

        #endregion
    }
}