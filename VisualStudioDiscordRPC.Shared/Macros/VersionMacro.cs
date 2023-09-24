﻿using EnvDTE;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public class VersionMacro : Macro
    {
        private readonly string _version;

        public VersionMacro(DTE dte) 
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
