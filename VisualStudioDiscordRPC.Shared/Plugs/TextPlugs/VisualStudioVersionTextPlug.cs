﻿using EnvDTE80;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class VisualStudioVersionTextPlug : BaseTextPlug
    {
        private readonly DTE2 _dte;

        public VisualStudioVersionTextPlug(DTE2 dte) 
        {
            _dte = dte;
        }

        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return string.Format("Visual Studio {0} {1}", _dte.Edition, VisualStudioHelper.GetVersionByDevNumber(_dte.Version));
        }
    }
}
