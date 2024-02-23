using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Variables;
using VisualStudioDiscordRPC.Shared.Observers;
using Newtonsoft.Json;
using System.Windows;
using System;

namespace VisualStudioDiscordRPC.Shared.Services
{
    public class VariableServiceConfig
    {
        [JsonProperty("variables")]
        public List<VariableInfo> Variables;
    }

    public class VariableInfo
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("type")]
        public string Type;
    }

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
        private readonly Dictionary<string, Variable> _variablesByType = new Dictionary<string, Variable>();
        private readonly Dictionary<string, VariableDescriptor> _variablesDescriptors = new Dictionary<string, VariableDescriptor>();

        public VariableService(VariableServiceConfig config, VsObserver vsObserver)
        {
            _vsObserver = vsObserver;

            RegisterVariable(new FileNameVariable(_vsObserver));
            RegisterVariable(new ProjectNameVariable(_vsObserver));
            RegisterVariable(new SolutionNameVariable(_vsObserver));
            RegisterVariable(new VersionVariable(_vsObserver.DTE));
            RegisterVariable(new EditionVariable(_vsObserver.DTE));
            RegisterVariable(new DebugModeVariable(_vsObserver.DTE));

            foreach (VariableInfo variableInfo in config.Variables)
            {
                if (!_variablesByType.TryGetValue(variableInfo.Type, out Variable variable))
                {
                    MessageBox.Show(
                        $"Can't initialize variable '{variableInfo.Name}': Type '{variableInfo.Type}' was not found or not registered",
                        "VisualStudioDiscordRPC",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    continue;
                }

                if (!TryInitVariable(variable, out Exception exception))
                {
                    MessageBox.Show(
                        $"Can't initialize variable '{variableInfo.Name}': An exception occurred during initialization.\n{exception}",
                        "VisualStudioDiscordRPC",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    continue;
                }

                _variablesDescriptors[variableInfo.Name] = new VariableDescriptor(variableInfo.Name, variableInfo.Description, variable);
            }
        }

        public Variable GetVariableByName(string name)
        {
            if (_variablesDescriptors.TryGetValue(name, out var variable))
                return variable.Variable;
            
            return null;
        }

        public IReadOnlyCollection<VariableDescriptor> GetVariablesDescriptors() => _variablesDescriptors.Values;

        private void RegisterVariable(Variable variable)
        {
            _variablesByType[variable.GetType().Name] = variable;
        }

        private bool TryInitVariable(Variable variable, out Exception exception)
        {
            exception = null;
            try
            {
                variable.Initialize();
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }
    }
}
