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
                IsEnableTagTotal = b;
                IsEnableTitleCaption = b;
                IsEnableKeyword = !b;
                IsEnableText = !b;
                if (b && (Target.Value == SearchTarget.Keyword || Target.Value == SearchTarget.Text))
                    Target.Value = SearchTarget.TagPartial;
                if (!b && (Target.Value == SearchTarget.TagTotal || Target.Value == SearchTarget.TitleCaption))
                    Target.Value = SearchTarget.TagPartial;
            });
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion

        #region IsEnableTagTotal

        private bool _isEnableTagTotal;

        public bool IsEnableTagTotal
        {
            get { return _isEnableTagTotal; }
            set { SetProperty(ref _isEnableTagTotal, value); }
        }

        #endregion

        #region IsEnableTitleCaption

        private bool _isEnableTitleCaption;

        public bool IsEnableTitleCaption
        {
            get { return _isEnableTitleCaption; }
            set { SetProperty(ref _isEnableTitleCaption, value); }
        }

        #endregion

        #region IsEnableText

        private bool _isEnableText;

        public bool IsEnableText
        {
            get { return _isEnableText; }
            set { SetProperty(ref _isEnableText, value); }
        }

        #endregion

        #region IsEnableKeyword

        private bool _isEnableKeyword;

        public bool IsEnableKeyword
        {
            get { return _isEnableKeyword; }
            set { SetProperty(ref _isEnableKeyword, value); }
        }

        #endregion
    }
}