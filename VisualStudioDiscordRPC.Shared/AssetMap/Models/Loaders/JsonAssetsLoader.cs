using System.Collections.Generic;
using System.IO;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using Newtonsoft.Json;

namespace VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders
{
    public class JsonAssetsLoader<T> : IAssetsLoader<T>
    {
        class JsonRootStructure
        {
            public T[] Assets { get; set; }
        }

        public IList<T> LoadAssets(string filepath)
        {
            string json = new StreamReader(filepath).ReadToEnd();
            
            T[] loadedAssets = JsonConvert.DeserializeObject<JsonRootStructure>(json).Assets;

            return loadedAssets;
        }
    }
}
