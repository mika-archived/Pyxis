using System.Collections.Generic;
using System.Numerics;

using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

using Microsoft.Graphics.Canvas.Effects;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Controls
{
    public sealed partial class BlurEffectControl : ContentControl
    {
        private readonly Compositor _compositor;

        private FrameworkElement BlurContentPresenter => GetTemplateChild("BlurContentPresenter") as FrameworkElement;

        public BlurEffectControl()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void ApplyBlur()
        {
            var graphicsEffect = new BlendEffect
            {
                Mode = BlendEffectMode.Multiply,
                Background = new ColorSourceEffect
                {
                    Name = "Tint",
                    Color = Colors.White
                },
                Foreground = new GaussianBlurEffect
                {
                    Name = "Blur",
                    Source = new CompositionEffectSourceParameter("Backdrop"),
                    BlurAmount = BlurAmount,
                    BorderMode = EffectBorderMode.Hard
                }
            };

            var blurEffectFactory = _compositor.CreateEffectFactory(graphicsEffect, new List<string> {"Blur.BlurAmount", "Tint.Color"});
            var brush = blurEffectFactory.CreateBrush();
            var destinationBrush = _compositor.CreateBackdropBrush();
            brush.SetSourceParameter("Backdrop", destinationBrush);

            var blurSprite = _compositor.CreateSpriteVisual();
            blurSprite.Size = new Vector2((float) BlurContentPresenter.ActualWidth, (float) BlurContentPresenter.ActualHeight);
            blurSprite.Brush = brush;

            ElementCompositionPreview.SetElementChildVisual(BlurContentPresenter, blurSprite);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            BlurContentPresenter.SizeChanged += OnSizeChanged;
            ApplyBlur();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var blurVisual = (SpriteVisual) ElementCompositionPreview.GetElementChildVisual(BlurContentPresenter);
            if (blurVisual != null)
                blurVisual.Size = e.NewSize.ToVector2();
        }

        #region BlurAmount

        public static readonly DependencyProperty BlurAmountProperty =
            DependencyProperty.Register(nameof(BlurAmount), typeof(int), typeof(BlurEffectControl), new PropertyMetadata(2));

        public int BlurAmount
        {
            get { return (int) GetValue(BlurAmountProperty); }
            set { SetValue(BlurAmountProperty, value); }
        }

        #endregion
    }
}