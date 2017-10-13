using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Xaml.Interactivity;

// ReSharper disable once CheckNamespace

namespace Pyxis.Controls
{
    /// <summary>
    ///     Scroll header control to be used with ListViews or GridViews
    /// </summary>
    public class ScrollHeaderEx : ContentControl
    {
        /// <summary>
        ///     Identifies the <see cref="Mode" /> property.
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(nameof(Mode), typeof(ScrollHeaderMode), typeof(ScrollHeaderEx),
                                        new PropertyMetadata(ScrollHeaderMode.None, OnModeChanged));

        /// <summary>
        ///     Identifies the <see cref="TargetListViewBase" /> property.
        /// </summary>
        public static readonly DependencyProperty TargetListViewBaseProperty =
            DependencyProperty.Register(nameof(TargetListViewBase), typeof(ListViewBase), typeof(ScrollHeaderEx),
                                        new PropertyMetadata(null, OnTargetListViewBaseChanged));

        public static readonly DependencyProperty TargetHeaderElementProperty =
            DependencyProperty.Register(nameof(TargetHeaderElement), typeof(UIElement), typeof(ScrollHeaderEx),
                                        new PropertyMetadata(null));

        /// <summary>
        ///     Gets or sets a value indicating whether the current mode.
        ///     Default is none.
        /// </summary>
        public ScrollHeaderMode Mode
        {
            get { return (ScrollHeaderMode) GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        /// <summary>
        ///     Gets or sets the container this header belongs to
        /// </summary>
        public ListViewBase TargetListViewBase
        {
            get { return (ListViewBase) GetValue(TargetListViewBaseProperty); }
            set { SetValue(TargetListViewBaseProperty, value); }
        }

        public UIElement TargetHeaderElement
        {
            get { return (UIElement) GetValue(TargetHeaderElementProperty); }
            set { SetValue(TargetHeaderElementProperty, value); }
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScrollHeaderEx" /> class.
        /// </summary>
        public ScrollHeaderEx()
        {
            DefaultStyleKey = typeof(ScrollHeaderEx);
            HorizontalContentAlignment = HorizontalAlignment.Stretch;
        }

        /// <summary>
        ///     Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        ///     <see cref="Control.ApplyTemplate" />.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            if (TargetListViewBase == null)
                return;

            // Place items below header
            var panel = TargetListViewBase.ItemsPanelRoot;
            Canvas.SetZIndex(panel, -1);
        }

        private static void OnTargetListViewBaseChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeaderEx)?.UpdateScrollHeaderBehavior();
        }

        private static void OnModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ScrollHeaderEx)?.UpdateScrollHeaderBehavior();
        }

        private void UpdateScrollHeaderBehavior()
        {
            if (TargetListViewBase == null)
                return;

            // Remove previous behaviors
            foreach (var behavior in Interaction.GetBehaviors(TargetListViewBase))
                if (behavior is FadeHeaderBehavior || behavior is QuickReturnHeaderBehavior || behavior is StickyHeaderBehavior)
                    Interaction.GetBehaviors(TargetListViewBase).Remove(behavior);

            switch (Mode)
            {
                case ScrollHeaderMode.None:
                    break;

                case ScrollHeaderMode.QuickReturn:
                    Interaction.GetBehaviors(TargetListViewBase).Add(new QuickReturnHeaderBehavior {HeaderElement = TargetHeaderElement});
                    break;

                case ScrollHeaderMode.Sticky:
                    Interaction.GetBehaviors(TargetListViewBase).Add(new StickyHeaderBehavior());
                    break;

                case ScrollHeaderMode.Fade:
                    Interaction.GetBehaviors(TargetListViewBase).Add(new FadeHeaderBehavior());
                    break;
            }
        }
    }
}