namespace VisualStudioDiscordRPC.Shared.Data
{
    public struct AssetInfo
    {
        public static readonly AssetInfo None = new AssetInfo(string.Empty, string.Empty);

        public readonly string Key;
        public readonly string Description;

        public AssetInfo(string key, string description)
        {
            Key = key;
            Description = description;
        }
    }
}
