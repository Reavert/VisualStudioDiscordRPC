using System;
using System.Collections.Generic;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class StringObserver
    {
        public event Action Changed;

        private readonly List<IObservableString> _textSources = new List<IObservableString>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void AddText(IObservableString textSource)
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

            foreach (IObservableString textSource in _textSources)
            {
                _stringBuilder.Append(textSource.Text);
            }

            return _stringBuilder.ToString();
        }
    }
}
