namespace VisualStudioDiscordRPC.Shared.Slots
{
    public struct AssetInfo
    {
        public string Key;
        public string Description;
    }

    public abstract class AssetSlot : AbstractSlot<AssetInfo>
    { }
}
