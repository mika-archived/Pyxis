using Windows.UI.Xaml;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    /// <summary>
    ///     <para>Target に指定したコントロールに対して、縦横比を維持したまま、最大サイズになるように</para>
    ///     <para>コントロールのサイズを調節します。</para>
    /// </summary>
    internal class UniformToParentControlBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register(nameof(Target), typeof(FrameworkElement), typeof(UniformToParentControlBehavior),
                                        new PropertyMetadata(null, PropertyChangedCallback));

        public FrameworkElement Target
        {
            get { return (FrameworkElement) GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((FrameworkElement) e.NewValue).SizeChanged +=
                ((UniformToParentControlBehavior) sender).ElementOnSizeChanged;
        }

        private void AssociatedObjectOnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (Target != null)
                Target.SizeChanged += ElementOnSizeChanged;
        }

        private void ElementOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Width ベース変換 (Desktop only)
            if (AssociatedObject == null || double.IsNaN(AssociatedObject.MaxHeight))
                return;
            var target = AssociatedObject;
            var aspectRatio = AssociatedObject.MaxHeight / AssociatedObject.MaxWidth;
            var gridSize = e.NewSize;
            target.Width = gridSize.Width;
            target.Height = gridSize.Width * aspectRatio;
        }

        #region Overrides of Behavior<FrameworkElement>

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DataContextChanged += AssociatedObjectOnDataContextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DataContextChanged -= AssociatedObjectOnDataContextChanged;
            Target.SizeChanged -= ElementOnSizeChanged;
        }

        #endregion
    }
}