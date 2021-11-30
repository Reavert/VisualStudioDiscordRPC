namespace VisualStudioDiscordRPC.Shared.AssetMap.Interfaces
{
    public interface IAssetComparer<T> where T : IAsset
    {
        bool Compare(T asset);
    }
}
