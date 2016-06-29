using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Dialogs
{
    public class FavoriteOptionDialogViewModel : DialogViewModel
    {
        private FavoriteOptionParameter _parameter;

        public ReactiveProperty<RestrictType> RestrictType { get; private set; }

        #region Overrides of DialogViewModel

        public override void OnInitialize(object parameter)
        {
            _parameter = parameter as FavoriteOptionParameter ?? new FavoriteOptionParameter();
            RestrictType = _parameter.ToReactivePropertyAsSynchronized(w => w.Restrict);
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion
    }
}