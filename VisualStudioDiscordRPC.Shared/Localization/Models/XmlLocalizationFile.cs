using System.Collections.Generic;
using System.Xml;
using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    public class XmlLocalizationFile : ILocalizationFile
    {
        public string LanguageName { get; }

        public string LocalizedLanguageName { get; }

        public IReadOnlyDictionary<string, string> LocalizedValues => _localizedValues;

        private Dictionary<string, string> _localizedValues = new Dictionary<string, string>();

        public XmlLocalizationFile(string filename)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            XmlNode languageNode = xmlDocument.DocumentElement;

            LanguageName = languageNode?.Attributes?["name"].Value;
            LocalizedLanguageName = languageNode?.Attributes?["localizedName"].Value;

            foreach (XmlNode translationNode in languageNode?.ChildNodes)
            {
                _localizedValues.Add(
                    translationNode.Attributes?["id"].Value,
                    translationNode.InnerText);
            }
        }
    }
}
