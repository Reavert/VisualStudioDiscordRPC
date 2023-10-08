using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Variables;
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

            RegisterVariable("file_name", "Name of the currently active file", new FileNameVariable(_vsObserver));
            RegisterVariable("project_name", "Name of the currently active project", new ProjectNameVariable(_vsObserver));
            RegisterVariable("solution_name", "Name of the currently active solution", new SolutionNameVariable(_vsObserver));
            RegisterVariable("version", "Current version of Visual Studio", new VersionVariable(_vsObserver.DTE));
            RegisterVariable("edition", "Current edition of Visual Studio", new EditionVariable(_vsObserver.DTE));
            RegisterVariable("debug_mode", "Current project debugging mode", new DebugModeVariable(_vsObserver.DTE));
        }

        public Variable GetVariableByName(string name)
        {
            if (_variables.TryGetValue(name, out var variable))
                return variable.Variable;
            
            return null;
        }

        public IReadOnlyCollection<VariableDescriptor> GetVariables() => _variables.Values;

        private void RegisterVariable(string name, string description, Variable variable)
        {
            _variables.Add(name, new VariableDescriptor(name, description, variable));
        }
    }
}
