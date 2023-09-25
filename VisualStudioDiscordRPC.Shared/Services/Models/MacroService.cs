using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Macros;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class MacroData
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Macro Macro { get; private set; }

        public MacroData(string name, string description, Macro macro)
        {
            Name = name;
            Description = description;
            Macro = macro;
        }
    }

    public class MacroService
    {
        private readonly VsObserver _vsObserver;
        private readonly Dictionary<string, MacroData> _macros = new Dictionary<string, MacroData>();

        public MacroService(VsObserver vsObserver)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _vsObserver = vsObserver;

            RegisterMacro("file_name", "The name of current active file", new FileNameMacro(_vsObserver));
            RegisterMacro("project_name", "The name of current active project", new ProjectNameMacro(_vsObserver));
            RegisterMacro("solution_name", "The name of current actie solution", new SolutionNameMacro(_vsObserver));
            RegisterMacro("version", "The version of Visual Studio", new VersionMacro(_vsObserver.DTE));
            RegisterMacro("edition", "The edition of Visual Studio", new EditionMacro(_vsObserver.DTE));
            RegisterMacro("debug_mode", "The current debugging mode", new DebugModeMacro(_vsObserver.DTE));
        }

        public Macro GetMacroByName(string name)
        {
            _macros.TryGetValue(name, out var macro);
            return macro.Macro;
        }

        public IReadOnlyCollection<MacroData> GetMacroDatas() => _macros.Values;

        private void RegisterMacro(string name, string description, Macro macro)
        {
            _macros.Add(name, new MacroData(name, description, macro));
        }
    }
}
