using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets
{
    public class ExtensionAsset : IAsset
    {
        public string Key { get; set; }

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
