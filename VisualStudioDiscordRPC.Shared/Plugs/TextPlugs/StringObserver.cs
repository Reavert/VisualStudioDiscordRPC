using System;
using System.Collections.Generic;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class StringObserver : IDisposable
    {
        public event Action Changed;

        private readonly List<IObservableString> _observableStrings = new List<IObservableString>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void AddText(IObservableString textSource)
        {
            _observableStrings.Add(textSource);
            textSource.Changed += OnAnyTextSourceChanged;
        }

        public void AddText(string staticText)
        {
            var staticTextSource = new StaticTextSource(staticText);
            AddText(staticTextSource);
        }

        public void Dispose()
        {
            foreach (IObservableString observableString in _observableStrings)
                observableString.Changed -= OnAnyTextSourceChanged;
        }

        private void OnAnyTextSourceChanged()
        {
            Changed?.Invoke();
        }

        public override string ToString()
        {
            _stringBuilder.Clear();

            foreach (IObservableString textSource in _observableStrings)
            {
                _stringBuilder.Append(textSource.Text);
            }

            return _stringBuilder.ToString();
        }
    }
}
