using System;
using VisualStudioDiscordRPC.Shared.Macros;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class ObservableMacro : IObservableString
    {
        public string Text => _data;

        public event Action Changed;

        private string _data;

        public ObservableMacro(Macro macro)
        {
            _data = macro.GetData();
            macro.Changed += OnMacroChanged;
        }

        private void OnMacroChanged(object sender, string data)
        {
            _data = data;
            Changed?.Invoke();
        }
    }
}
