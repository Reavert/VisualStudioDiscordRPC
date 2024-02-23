using EnvDTE80;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class VersionVariable : Variable
    {
        private readonly DTE2 _dte;
        private string _version;

        public VersionVariable(DTE2 dte) 
        {
            _dte = dte;
        }

        public override void Initialize()
        {
            _version = _dte.Version;
        }

        public override string GetData()
        {
            return VisualStudioHelper.GetVersionByDevNumber(_version);
        }
    }
}
