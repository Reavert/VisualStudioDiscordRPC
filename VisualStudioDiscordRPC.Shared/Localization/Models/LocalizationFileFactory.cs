using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    public class LocalizationFileFactory
    {
        private System.Type GetAcceptableLocalizationFile(string filepath)
        {
            if (filepath == null)
            {
                throw new System.ArgumentNullException(nameof(filepath));
            }

            if (filepath.EndsWith(".json", System.StringComparison.InvariantCulture))
            {
                return typeof(JsonLocalizationFile);
            }

            if (filepath.EndsWith(".xml", System.StringComparison.InvariantCulture))
            {
                return typeof(XmlLocalizationFile);
            }

            throw new System.Exception($"No acceptable type for {filepath}");
        }

        public ILocalizationFile CreateLocalizationFile(string filename)
        {
            System.Type acceptableType = GetAcceptableLocalizationFile(filename);

            var acceptableFile = (ILocalizationFile)acceptableType.GetConstructor(
                new[] { typeof(string) })
                ?.Invoke(new object[] { filename });

            return acceptableFile;
        }
    }
}
