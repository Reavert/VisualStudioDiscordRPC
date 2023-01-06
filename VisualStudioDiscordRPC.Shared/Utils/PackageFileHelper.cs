using System;
using System.IO;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class PackageFileHelper
    {
        public static string GetPackageFilePath(string filename)
        {
            string codebase = typeof(VisualStudioDiscordRPCPackage).Assembly.Location;
            var uri = new Uri(codebase, UriKind.Absolute);

            string installationPath = Path.GetDirectoryName(uri.LocalPath);

            return Path.Combine(installationPath, filename);
        }
    }
}
