using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Interfaces
{
    public interface IAssetMap<T> where T : IAsset
    {
        IList<T> Assets { get; set; }

        string GetAssetKey(IAssetComparer<T> assetComparer);
        IAsset GetAsset(string key);
    }
}
