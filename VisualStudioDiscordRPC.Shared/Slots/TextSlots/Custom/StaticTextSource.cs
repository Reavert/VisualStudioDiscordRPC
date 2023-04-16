using System;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class StaticTextSource : ITextSource
    {
        public string Name => "static";

        public string Text => _text;

        public event Action Changed;

        private string _text;

        public StaticTextSource(string text)
        {
            _text = text;
            Changed?.Invoke();
        }
    }
}
