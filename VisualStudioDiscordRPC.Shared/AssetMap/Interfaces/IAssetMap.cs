using System;
using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Interfaces
{
    public interface IAssetMap<T> where T : IAsset
    {
        IList<T> Assets { get; set; }

        T GetAsset(Func<T, bool> condition);
    }
}
