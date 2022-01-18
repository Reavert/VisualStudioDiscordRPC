using System.Collections.Generic;
using System.Xml;

namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    public class XmlLocalizationFile : LocalizationFile
    {
        public XmlLocalizationFile(string filename)
        {
            LocalizedValues = new Dictionary<string, string>();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            XmlNode languageNode = xmlDocument.DocumentElement;

            LanguageName = languageNode?.Attributes?["name"].Value;
            LocalizedLanguageName = languageNode?.Attributes?["localizedName"].Value;

            foreach (XmlNode translationNode in languageNode?.ChildNodes)
            {
                LocalizedValues.Add(
                    translationNode.Attributes?["id"].Value,
                    translationNode.InnerText);
            }
        }
    }
}
