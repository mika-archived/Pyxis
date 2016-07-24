using Pyxis.Models.Enums;
using Pyxis.Models.Parameters;
using Pyxis.Mvvm;
using Pyxis.ViewModels.Base;

using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Pyxis.ViewModels.Dialogs
{
    public class AdvancedSearchOptionDialogViewModel : DialogViewModel
    {
        private SearchOptionParameter _parameter;

        public ReactiveProperty<string> EitherWord { get; private set; }
        public ReactiveProperty<string> IgnoreWord { get; private set; }
        public ReactiveProperty<string> BookmarkCount { get; private set; }
        public ReactiveProperty<string> ViewCount { get; private set; }
        public ReactiveProperty<string> CommentCount { get; private set; }
        public ReactiveProperty<string> PageCount { get; private set; }
        public ReactiveProperty<string> Height { get; private set; }
        public ReactiveProperty<string> Width { get; private set; }
        public ReactiveProperty<string> Tool { get; private set; }
        public ReactiveProperty<string> TextLength { get; private set; }

        public bool IsEnabledIllustMode => _parameter.SearchType == SearchType.IllustsAndManga;
        public bool IsEnabledNovelMode => !IsEnabledIllustMode;

        #region Overrides of DialogViewModel

        public override void OnInitialize(object parameter)
        {
            _parameter = parameter as SearchOptionParameter;
            EitherWord = _parameter.ToReactivePropertyAsSynchronized(w => w.EitherWord).AddTo(this);
            IgnoreWord = _parameter.ToReactivePropertyAsSynchronized(w => w.IgnoreWord).AddTo(this);
            BookmarkCount = _parameter.ToReactivePropertyAsSynchronized(w => w.BookmarkCount,
                                                                        w => w.ToString(),
                                                                        int.Parse).AddTo(this);
            ViewCount = _parameter.ToReactivePropertyAsSynchronized(w => w.ViewCount,
                                                                    w => w.ToString(),
                                                                    int.Parse).AddTo(this);
            CommentCount = _parameter.ToReactivePropertyAsSynchronized(w => w.CommentCount,
                                                                       w => w.ToString(),
                                                                       int.Parse).AddTo(this);
            PageCount = _parameter.ToReactivePropertyAsSynchronized(w => w.PageCount,
                                                                    w => w.ToString(),
                                                                    int.Parse).AddTo(this);
            Height = _parameter.ToReactivePropertyAsSynchronized(w => w.Height,
                                                                 w => w.ToString(),
                                                                 int.Parse).AddTo(this);
            Width = _parameter.ToReactivePropertyAsSynchronized(w => w.Width,
                                                                w => w.ToString(),
                                                                int.Parse).AddTo(this);
            Tool = _parameter.ToReactivePropertyAsSynchronized(w => w.Tool).AddTo(this);
            TextLength = _parameter.ToReactivePropertyAsSynchronized(w => w.TextLength,
                                                                     w => w.ToString(),
                                                                     int.Parse).AddTo(this);
        }

        public override void OnFinalize() => ResultValue = _parameter;

        #endregion
    }
}