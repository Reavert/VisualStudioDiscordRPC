using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Interfaces
{
    public interface IAssetsLoader<T>
    {
        IList<T> LoadAssets(string filepath);
    }
}
