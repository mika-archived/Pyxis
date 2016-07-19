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
            var senderThis = sender as UniformToParentControlBehavior;
            var element = sender.GetValue(TargetProperty) as FrameworkElement;
            if (element == null || senderThis == null)
                return;
            element.SizeChanged += (sender1, e1) =>
            {
                // Width ベース変換
                if (senderThis.AssociatedObject == null || double.IsNaN(senderThis.AssociatedObject.MaxHeight))
                    return;
                var target = senderThis.AssociatedObject;
                var aspectRatio = senderThis.AssociatedObject.MaxHeight / senderThis.AssociatedObject.MaxWidth;
                var gridSize = e1.NewSize;
                target.Width = gridSize.Width;
                target.Height = gridSize.Width * aspectRatio;
            };
        }
    }
}