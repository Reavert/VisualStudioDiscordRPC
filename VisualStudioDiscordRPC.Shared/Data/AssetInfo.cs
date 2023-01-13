namespace VisualStudioDiscordRPC.Shared.Data
{
    public struct AssetInfo
    {
        public readonly string Key;
        public readonly string Description;

        public AssetInfo(string key, string description)
        {
            Key = key;
            Description = description;
        }
    }
}
