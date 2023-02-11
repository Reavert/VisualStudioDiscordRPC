using System;
using System.Collections.Generic;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class CustomString
    {
        public event Action Changed;

        private readonly List<ITextSource> _textSources = new List<ITextSource>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void AddText(ITextSource textSource)
        {
            _textSources.Add(textSource);
            textSource.Changed += OnAnyTextSourceChanged;
        }

        public void AddText(string staticText)
        {
            var staticTextSource = new StaticTextSource(staticText);
            AddText(staticTextSource);
        }

        private void OnAnyTextSourceChanged()
        {
            Changed?.Invoke();
        }

        public override string ToString()
        {
            _stringBuilder.Clear();

            foreach (ITextSource textSource in _textSources)
            {
                _stringBuilder.Append(textSource.Text);
            }

            return _stringBuilder.ToString();
        }
    }
}
