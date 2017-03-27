using System;
using System.Linq;

using Microsoft.EntityFrameworkCore.Internal;

using Sagitta.Models;

namespace Pyxis.ViewModels.Contents
{
    public class NovelViewModel : ContentViewModel
    {
        private readonly Novel _novel;

        public override Uri Thumbnail => new Uri(_novel.ImageUrls.Medium);
        public string Tags => _novel.Tags.Take(5).Select(w => w.Name).Join();
        public string TextLength => $"{_novel.TextLength:##,###}文字";

        public NovelViewModel(Novel novel) : base(novel)
        {
            _novel = novel;
        }
    }
}