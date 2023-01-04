using System;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets
{
    public class VisualStudioVersionAsset : Asset
    { 
        public string Version;

        public VisualStudioVersionAsset(string key, string version)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }
    }
}
