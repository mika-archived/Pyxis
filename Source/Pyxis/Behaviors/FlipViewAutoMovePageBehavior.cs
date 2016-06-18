using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

using Microsoft.Xaml.Interactivity;

namespace Pyxis.Behaviors
{
    internal class FlipViewAutoMovePageBehavior : Behavior<FlipView>
    {
        private int _currentIndex;
        private IDisposable _disposable;
        private IDisposable _privateDisposable;

        private async Task MoveFlip()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var count = AssociatedObject.Items?.Count ?? 0;
                _currentIndex = AssociatedObject.SelectedIndex;
                if (_currentIndex + 1 >= count)
                    _currentIndex = 0;
                else
                    _currentIndex++;
                if (AssociatedObject.SelectedIndex < 0)
                    return;
                if (_currentIndex == 0)
                {
                    _currentIndex = AssociatedObject.SelectedIndex;
                    _privateDisposable =
                        Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(5))
                                  .Subscribe(async w => await ResetPosition());
                }
                else
                    AssociatedObject.SelectedIndex = _currentIndex;
            });
        }

        private async Task ResetPosition()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (--_currentIndex >= 0)
                    AssociatedObject.SelectedIndex = _currentIndex;
                else
                    _privateDisposable?.Dispose();
            });
        }

        #region Overrides of Behavior

        protected override void OnAttached()
        {
            base.OnAttached();
            _disposable =
                Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(10)).Subscribe(async w => await MoveFlip());
        }

        protected override void OnDetaching()
        {
            _disposable.Dispose();
            base.OnDetaching();
        }

        #endregion
    }
}