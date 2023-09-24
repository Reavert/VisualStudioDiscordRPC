using EnvDTE;
using EnvDTE80;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Macros;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class MacroService
    {
        
        private readonly VsObserver _vsObserver;
        private readonly Dictionary<string, Macro> _macros;

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
                { "debug_mode", new DebugModeMacro(_vsObserver.DTE) }
        };
        }

        public Macro GetMacroByName(string name)
        {
            _macros.TryGetValue(name, out var macro);
            return macro;
        }
    }
}
