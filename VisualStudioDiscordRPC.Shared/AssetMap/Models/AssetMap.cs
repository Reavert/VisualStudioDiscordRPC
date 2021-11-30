using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models
{
    public class AssetMap<T> : IAssetMap<T> where T : IAsset
    {
        public IList<T> Assets { get; set; }

        public AssetMap()
        {
            Assets = new List<T>();
        }

        public IAsset GetAsset(string key)
        {
            return Assets.FirstOrDefault(asset => asset.Key == key);
        }

        public string GetAssetKey(IAssetComparer<T> assetComparer)
        {
            var foundAsset = Assets.FirstOrDefault(asset => assetComparer.Compare(asset));

            return foundAsset == null ? "text_file" : foundAsset.Key;
        }
    }
}
