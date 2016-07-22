using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class BrowsingHistoryService : IBrowsingHistoryService, IDisposable
    {
        private readonly List<int> _illustIds;
        private readonly List<int> _novelIds;
        private readonly IPixivClient _pixivClient;
        private IDisposable _disposable;

        public BrowsingHistoryService(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            _illustIds = new List<int>();
            _novelIds = new List<int>();
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        #endregion

        public void Start()
        {
            _disposable = Observable.Timer(TimeSpan.FromMinutes(2)).Subscribe(async w => await SendAsync());
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SendAsync()
        {
            if (_illustIds.Count > 0)
            {
                await _pixivClient.User.BrowsingHistory.Illust.AddAsync(illust_ids => string.Join(",", _illustIds));
                _illustIds.Clear();
            }
            if (_novelIds.Count > 0)
            {
                await _pixivClient.User.BrowsingHistory.Novel.AddAsync(novel_ids => string.Join(",", _novelIds));
                _novelIds.Clear();
            }
        }

        #region Implementation of IBrowsingHistoryService

        public void Add(IIllust illust)
        {
            if (_illustIds.Contains(illust.Id))
                return;
            _illustIds.Add(illust.Id);
        }

        public void Add(INovel novel)
        {
            if (_novelIds.Contains(novel.Id))
                return;
            _novelIds.Add(novel.Id);
        }

        #endregion
    }
}