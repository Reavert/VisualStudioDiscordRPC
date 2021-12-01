using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets
{
    public abstract class Asset : IAsset
    {
        public string Key { get; protected set; }
    }
}
