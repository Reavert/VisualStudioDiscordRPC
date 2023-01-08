using System;
using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets
{
    public class ExtensionAsset : Asset
    {
        public static readonly ExtensionAsset Default =
            new ExtensionAsset("unknown_file", "Unknown", new string[] { });

        public string Name { get; set; }

        public IList<string> Extensions { get; set; }

        public ExtensionAsset(string key, string name, IList<string> extensions)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Extensions = extensions ?? throw new ArgumentException(nameof(extensions));
        }
    }
}
