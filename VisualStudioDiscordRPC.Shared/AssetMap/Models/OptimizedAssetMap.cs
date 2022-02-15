using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models
{
    public class OptimizedAssetMap<T> : IAssetMap<T> where T : IAsset
    {
        public IList<T> Assets { get; set; }

        private int _distance;

        public T GetAsset(Func<T, bool> predicate)
        {
            int i = Assets.FindIndex(predicate);

            if (i == -1)
            {
                return default;
            }

            T item = Assets[i];

            if (i > _distance)
            {
                Assets.RemoveAt(i);
                Assets.Insert(0, item);

                _distance++;
            }
            
            return item;
        }
    }
}
