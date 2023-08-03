using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Macros;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class MacroService
    {
        
        private VsObserver _vsObserver;
        private Dictionary<string, Macro> _macros;

        public MacroService(VsObserver vsObserver)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _vsObserver = vsObserver;
            _macros = new Dictionary<string, Macro>()
            {
                { "file_name", new FileNameMacro(_vsObserver) },
                { "project_name", new ProjectNameMacro(_vsObserver) },
                { "solution_name", new SolutionNameMacro(_vsObserver) },
                { "version", new VersionMacro(_vsObserver.DTE) },
                { "edition", new EditionMacro(_vsObserver.DTE) },
                { "debug", new DebugMacro(_vsObserver.DTE.Debugger, _vsObserver.DTE.Events.DebuggerEvents) }
        };
        }

        public Macro Instantiate(string name)
        {
            _macros.TryGetValue(name, out var macro);
            return macro;
        }
    }
}
