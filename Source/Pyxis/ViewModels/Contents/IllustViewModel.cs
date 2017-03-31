using System;
using System.Linq;

using Pyxis.Models.Parameters;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class IllustViewModel : ContentViewModel
    {
        private readonly Illust _illust;

        public override Uri Thumbnail => new Uri(_illust.ImageUrls.SquareMedium ?? _illust.MetaPages.First().ImageUrls.SquareMedium);
        public bool HasMultiPage => _illust.PageCount > 1;
        public int PageCount => HasMultiPage ? _illust.PageCount : 1;

        public IllustViewModel(Illust illust) : base(illust)
        {
            _illust = illust;
        }

        public override void NavigateTo() => NavigateTo("Illust", new IllustParameter {Illust = _illust});
    }
}