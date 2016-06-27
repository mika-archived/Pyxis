using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Dialogs
{
    public class SearchOptionDialogViewModel : DialogViewModel
    {
        private SearchOptionParameter _parameter;

        public ReactiveProperty<SearchType> ContentType { get; private set; }

        public ReactiveProperty<SearchTarget> Target { get; private set; }

        public ReactiveProperty<SearchDuration> Duration { get; private set; }

        #region Overrides of DialogViewModel

        public override void OnInitialize(object parameter)
        {
            _parameter = parameter as SearchOptionParameter ?? new SearchOptionParameter();
            ContentType = _parameter.ToReactivePropertyAsSynchronized(w => w.SearchType);
            Target = _parameter.ToReactivePropertyAsSynchronized(w => w.Target);
            Duration = _parameter.ToReactivePropertyAsSynchronized(w => w.Duration);
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion
    }
}