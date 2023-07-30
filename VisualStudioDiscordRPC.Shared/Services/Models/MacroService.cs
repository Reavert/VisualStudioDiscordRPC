using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Macros;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class MacroService
    {
        private VsObserver _vsObserver;
        public MacroService(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;
        }

        public Macro Instantiate(string name)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            switch (name)
            {
                case "file_name": return new FileNameMacro(_vsObserver);
                case "project_name": return new ProjectNameMacro(_vsObserver);
                case "solution_name": return new SolutionNameMacro(_vsObserver);
                case "version": return new VersionMacro(_vsObserver.DTE);
                case "edition": return new EditionMacro(_vsObserver.DTE);
                case "debug": return new DebugMacro(_vsObserver.DTE.Debugger, _vsObserver.DTE.Events.DebuggerEvents);
                default: return null;
            }
        }
    }
}
