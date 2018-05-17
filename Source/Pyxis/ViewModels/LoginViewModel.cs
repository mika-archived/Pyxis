﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Reactive.Bindings;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        private readonly PixivClient _pixivClient;
        private List<Illust> _illustCollection;
        public ReactiveProperty<Uri> Background1 { get; set; }
        public ReactiveProperty<Uri> Background2 { get; set; }

        public LoginViewModel(PixivClient pixivClient)
        {
            _pixivClient = pixivClient;

            Background1 = new ReactiveProperty<Uri>();
            Background2 = new ReactiveProperty<Uri>();

            Task.Run(LoadBackgrounds);
        }

        private async Task LoadBackgrounds()
        {
            _illustCollection = (await _pixivClient.Walkthrough.IllustsAsync()).Illusts.ToList();
            Background1.Value = new Uri(_illustCollection[0].ImageUrls.Medium);
        }
    }
}