using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

using IIllustBookmarkApiV2 = Pyxis.Beta.Interfaces.Rest.v2.IIllustBookmarkApi;

namespace Pyxis.Beta.Interfaces.Rest.v2
{
    public interface IIllustApi
    {
        IIllustBookmarkApiV2 Bookmark { get; }

        Task<IIllusts> FollowAsync(params Expression<Func<string, object>>[] parameters);

        Task<IIllusts> MypixivAsync(params Expression<Func<string, object>>[] parameters);
    }
}