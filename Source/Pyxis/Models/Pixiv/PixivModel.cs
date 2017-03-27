using Prism.Mvvm;

using Sagitta;

namespace Pyxis.Models.Pixiv
{
    public abstract class PixivModel : BindableBase
    {
        protected PixivClient PixivClient { get; }

        protected PixivModel(PixivClient pixivClient)
        {
            PixivClient = pixivClient;
        }
    }
}