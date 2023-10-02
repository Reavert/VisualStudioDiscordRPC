using System;
using VisualStudioDiscordRPC.Shared.Macros;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class ObservableVariable : IObservableString
    {
        public string Text => _data;

        public event Action Changed;

        private string _data;

        public ObservableVariable(Variable variable)
        {
            _data = variable.GetData();
            variable.Changed += OnVariableChanged;
        }

        private void OnVariableChanged(object sender, string data)
        {
            _data = data;
            Changed?.Invoke();
        }
    }
}
