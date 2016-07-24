using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;
using Pyxis.Beta.Interfaces.Rest;
using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

namespace Pyxis.Services
{
    internal class BrowsingHistoryService : IBrowsingHistoryService
    {
        private readonly List<int> _illustIds;
        private readonly List<int> _novelIds;
        private readonly IPixivClient _pixivClient;

        public BrowsingHistoryService(IPixivClient pixivClient)
        {
            _pixivClient = pixivClient;
            _illustIds = new List<int>();
            _novelIds = new List<int>();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private async Task SendAsync(bool isIllust = true)
        {
            if (isIllust)
            {
                await _pixivClient.User.BrowsingHistory.Illust.AddAsync(illust_ids => string.Join(",", _illustIds));
                _illustIds.Clear();
            }
            else
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
            if (_illustIds.Count >= 5)
                RunHelper.RunAsync(SendAsync, true);
        }

        public void Add(INovel novel)
        {
            if (_novelIds.Contains(novel.Id))
                return;
            _novelIds.Add(novel.Id);
            if (_novelIds.Count >= 5)
                RunHelper.RunAsync(SendAsync, false);
        }

        #endregion
    }
}