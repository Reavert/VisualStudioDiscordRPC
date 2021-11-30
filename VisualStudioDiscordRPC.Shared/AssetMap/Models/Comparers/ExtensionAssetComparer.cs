using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models
{
    public class ExtensionAssetComparer : IAssetComparer<ExtensionAsset>
    {
        public string RequiredExtension { get; set; }

        public bool Compare(ExtensionAsset asset)
        {
            return asset.Extensions.Contains(RequiredExtension);
        }
    }
}
