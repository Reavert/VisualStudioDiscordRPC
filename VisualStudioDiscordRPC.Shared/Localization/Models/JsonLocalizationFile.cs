namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

    public class JsonLocalizationFile : ILocalizationFile
    {
        public string LanguageName { get; }

        public string LocalizedLanguageName { get; }

        public IReadOnlyDictionary<string, string> LocalizedValues { get; }

        public JsonLocalizationFile(string filename)
        {
            var file = new StreamReader(filename);
            string json = file.ReadToEnd();
            file.Close();

            LanguageName = Path.GetFileNameWithoutExtension(filename);
            LocalizedLanguageName = LanguageName;
            LocalizedValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
    }
}
