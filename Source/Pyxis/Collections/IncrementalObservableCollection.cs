using System.Collections.ObjectModel;

using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace Pyxis.Collections
{
    public class IncrementalObservableCollection<T> : ObservableCollection<T>, ISupportIncrementalLoading
        where T : class
    {
        public ISupportIncrementalLoading SupportIncrementalLoading { get; set; }

        #region Implementation of ISupportIncrementalLoading

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
            => SupportIncrementalLoading.LoadMoreItemsAsync(count);

        public bool HasMoreItems => SupportIncrementalLoading?.HasMoreItems ?? false;

        #endregion
    }
}