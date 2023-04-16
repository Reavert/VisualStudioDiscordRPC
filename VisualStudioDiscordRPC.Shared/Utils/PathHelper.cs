using System;
using System.IO;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class PathHelper
    {
        public const string AppDataFolderName = "VisualStudioDiscordRPC";

        static PathHelper()
        {
            if (!Directory.Exists(GetApplicationDataPath()))
            {
                Directory.CreateDirectory(GetApplicationDataPath());
            }
        }

        public static string GetPackageInstallationPath(string path)
        {
            string codebase = typeof(VisualStudioDiscordRPCPackage).Assembly.Location;
            var uri = new Uri(codebase, UriKind.Absolute);

            string installationPath = Path.GetDirectoryName(uri.LocalPath);

            return Path.Combine(installationPath, path);
        }

        public static string GetApplicationDataPath()
        {
            return Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                AppDataFolderName);
        }
    }
}
