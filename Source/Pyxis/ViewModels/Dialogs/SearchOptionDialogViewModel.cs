using System;

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
            ContentType.Subscribe(w =>
            {
                var b = w != SearchType.Novels;
                IsEnabledIllustMode = w == SearchType.IllustsAndManga;
                IsEnabledNovelMode = !IsEnabledIllustMode;
                if (b && (Target.Value == SearchTarget.Keyword || Target.Value == SearchTarget.Text))
                    Target.Value = SearchTarget.TagPartial;
                if (!b && (Target.Value == SearchTarget.TagTotal || Target.Value == SearchTarget.TitleCaption))
                    Target.Value = SearchTarget.TagPartial;
            });
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion

        #region IsEnabledIllustMode

        private bool _isEnabledIllustMode;

        public bool IsEnabledIllustMode
        {
            get { return _isEnabledIllustMode; }
            set { SetProperty(ref _isEnabledIllustMode, value); }
        }

        #endregion

        #region IsEnabledNovelMode

        private bool _isEnabledNovelMode;

        public bool IsEnabledNovelMode
        {
            get { return _isEnabledNovelMode; }
            set { SetProperty(ref _isEnabledNovelMode, value); }
        }

        #endregion
    }
}