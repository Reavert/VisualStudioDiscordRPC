using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models
{
    public class AssetMap<T> : IAssetMap<T> where T : IAsset
    {
        public IList<T> Assets { get; set; }

        public T GetAsset(Func<T, bool> predicate)
        {
            return Assets.FirstOrDefault(predicate);
        }
    }
}
