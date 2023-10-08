using EnvDTE80;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class VersionVariable : Variable
    {
        private readonly string _version;

        public VersionVariable(DTE2 dte) 
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            _version = dte.Version;
        }

        public override string GetData()
        {
            return VisualStudioHelper.GetVersionByDevNumber(_version);
        }
    }
}
