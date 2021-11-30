using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Comparers
{
    public class DefaultAssetComparer : IAssetComparer<IAsset>
    {
        public string RequiredKey { get; set; }

        public bool Compare(IAsset asset)
        {
            return asset.Key == RequiredKey;
        }
    }
}
