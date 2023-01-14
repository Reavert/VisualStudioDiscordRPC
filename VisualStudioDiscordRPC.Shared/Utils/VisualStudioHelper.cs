using System.Collections.Generic;
using System.Xml;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class VisualStudioHelper
    {
        private static Dictionary<string, string> VsVersions = new Dictionary<string, string>
        {
            { "16", "2019" },
            { "17", "2022" }
        };

        public static string GetVersionByDevNumber(string version)
        {
            string majorVersion = version.Split('.')[0];
            return VsVersions[majorVersion];
        }

        public static string GetExtensionVersion()
        {
            const string vsixManifesetFileName = "extension.vsixmanifest";

            var vsixManifest = new XmlDocument();

            string vsixManifestPath = PackageFileHelper.GetPackageFilePath(vsixManifesetFileName);
            vsixManifest.Load(vsixManifestPath);

            return vsixManifest.DocumentElement["Metadata"]["Identity"].Attributes["Version"].Value;
        }
    }
}
