using System;
using System.Linq;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class IllustViewModel : ContentViewModel
    {
        private readonly Illust _illust;

        public override Uri Thumbnail => new Uri(_illust.ImageUrls.SquareMedium ?? _illust.MetaPages.First().ImageUrls.SquareMedium);
        public bool HasMultiPage => _illust.MetaSinglePage != null;
        public int PageCount => HasMultiPage ? _illust.MetaPages.Count() : 0;

        public IllustViewModel(Illust illust) : base(illust)
        {
            _illust = illust;
        }
    }
}