using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Macros;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services
{
    public class VariableDescriptor
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Variable Variable { get; private set; }

        public VariableDescriptor(string name, string description, Variable variable)
        {
            Name = name;
            Description = description;
            Variable = variable;
        }
    }

    public class VariableService
    {
        private readonly VsObserver _vsObserver;
        private readonly Dictionary<string, VariableDescriptor> _variables = new Dictionary<string, VariableDescriptor>();

        public VariableService(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;

            RegisterVariable("file_name", "The name of current active file", new FileNameVariable(_vsObserver));
            RegisterVariable("project_name", "The name of current active project", new ProjectNameVariable(_vsObserver));
            RegisterVariable("solution_name", "The name of current actie solution", new SolutionNameVariable(_vsObserver));
            RegisterVariable("version", "The version of Visual Studio", new VersionVariable(_vsObserver.DTE));
            RegisterVariable("edition", "The edition of Visual Studio", new EditionVariable(_vsObserver.DTE));
            RegisterVariable("debug_mode", "The current debugging mode", new DebugModeVariable(_vsObserver.DTE));
        }

        public Variable GetVariableByName(string name)
        {
            _variables.TryGetValue(name, out var variable);
            return variable.Variable;
        }

        public IReadOnlyCollection<VariableDescriptor> GetVariables() => _variables.Values;

        private void RegisterVariable(string name, string description, Variable variable)
        {
            _variables.Add(name, new VariableDescriptor(name, description, variable));
        }
    }
}
