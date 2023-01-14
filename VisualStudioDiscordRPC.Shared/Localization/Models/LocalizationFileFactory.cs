using System;
using System.Collections.Generic;
using System.IO;
using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    public class LocalizationFileFactory
    {
        private readonly Dictionary<string, Func<string, ILocalizationFile>> _localizedFilesExtensions =
            new Dictionary<string, Func<string, ILocalizationFile>>
            {
                {".json", filename => new JsonLocalizationFile(filename)},
                {".xml", filename => new XmlLocalizationFile(filename)}
            };

        public ILocalizationFile CreateLocalizationFile(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            string extension = Path.GetExtension(filename);

            if (!_localizedFilesExtensions.ContainsKey(extension))
            {
                throw new KeyNotFoundException($"Not found localization type for {extension}");
            }

            return _localizedFilesExtensions[extension].Invoke(filename);
        }
    }
}
