using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models
{
    public class AssetMap<T> : IAssetMap<T> where T : IAsset
    {
        public IList<T> Assets { get; set; }

        public AssetMap()
        {
            Assets = new List<T>();
        }

        public T GetAsset(IAssetComparer<T> assetComparer)
        {
            return Assets.FirstOrDefault(asset => assetComparer.Compare(asset));
        }
    }
}
