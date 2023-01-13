using EnvDTE;
using System.Collections.Generic;

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
    }
}
