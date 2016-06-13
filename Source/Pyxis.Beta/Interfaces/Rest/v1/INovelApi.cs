using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Pyxis.Beta.Interfaces.Models.v1;

namespace Pyxis.Beta.Interfaces.Rest.v1
{
    public interface INovelApi
    {
        Task<INovels> FollowAsync(params Expression<Func<string, object>>[] parameters);

        Task<INovels> MypixivAsync(params Expression<Func<string, object>>[] parameters);

        Task<INovels> NewAsync(params Expression<Func<string, object>>[] parameters);

        Task<INovels> RankingAsync(params Expression<Func<string, object>>[] parameters);

        Task<IRecommendedNovels> RecommendedAsync(params Expression<Func<string, object>>[] parameters);

        Task<IRecommendedNovels> RecommendedNologinAsync(params Expression<Func<string, object>>[] parameters);
    }
}