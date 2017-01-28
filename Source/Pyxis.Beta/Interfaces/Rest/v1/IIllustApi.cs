using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface IIllustApi
    {
        IIllustBookmarkApi Bookmark { get; }

        Task<IComments> CommentsAsync(params Expression<Func<string, object>>[] parameters);

        Task<IIllust> DetailAsync(params Expression<Func<string, object>>[] parameters);

        Task<IIllusts> NewAsync(params Expression<Func<string, object>>[] parameters);

        Task<IIllusts> RankingAsync(params Expression<Func<string, object>>[] parameters);

        Task<IRecommendedIllusts> RecommendedAsync(params Expression<Func<string, object>>[] parameters);

        Task<IRecommendedIllusts> RecommendedNologinAsync(params Expression<Func<string, object>>[] parameters);
    }
}