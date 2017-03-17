using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using Pyxis.Helpers;
using Pyxis.Services.Interfaces;

using Sagitta;
using Sagitta.Models;

namespace Pyxis.Services
{
    internal class BrowsingHistoryService : IBrowsingHistoryService
    {
        private readonly List<int> _illustIds;
        private readonly List<int> _novelIds;
        private readonly PixivClient _pixivClient;

        public BrowsingHistoryService(PixivClient pixivClient)
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
                await _pixivClient.User.BrowsingHistory.AddIllustAsync(_illustIds);
                _illustIds.Clear();
            }
            else
            {
                await _pixivClient.User.BrowsingHistory.AddNovelAsync(_novelIds);
                _novelIds.Clear();
            }
        }

        #region Implementation of IBrowsingHistoryService

        public void Add(Illust illust)
        {
            if (_illustIds.Contains(illust.Id))
                return;
            _illustIds.Add(illust.Id);
            if (_illustIds.Count >= 5)
                RunHelper.RunAsync(SendAsync, true);
        }

        public void Add(Novel novel)
        {
            if (_novelIds.Contains(novel.Id))
                return;
            _novelIds.Add(novel.Id);
            if (_novelIds.Count >= 5)
                RunHelper.RunAsync(SendAsync, false);
        }

        public void ForcePush()
        {
            if (_illustIds.Count >= 1)
                RunHelper.RunAsync(SendAsync, true);
            if (_novelIds.Count >= 1)
                RunHelper.RunAsync(SendAsync, false);
        }

        #endregion Implementation of IBrowsingHistoryService
    }
}