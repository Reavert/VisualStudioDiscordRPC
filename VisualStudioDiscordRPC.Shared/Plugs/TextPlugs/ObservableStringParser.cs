using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class ObservableStringParser
    {
        public enum EntryType
        {
            Text,
            Keyword
        }

        public readonly struct Entry
        {
            public readonly EntryType Type;
            public readonly string Value;

            public Entry(EntryType type, string value)
            {
                Type = type;
                Value = value;
            }
        }

        private StringBuilder _stringBuilder = new StringBuilder();
        private StringReader _stringReader;

        private bool _isVariable;

        public List<Entry> Parse(string text)
        {
            var entries = new List<Entry>();

            _stringReader = new StringReader(text);
            _stringBuilder.Clear();
            _isVariable = false;

            while (true)
            {
                int symbol = _stringReader.Read();
                var symbolChar = (char)symbol;

                if (symbolChar == '{')
                {
                    if (!_isVariable)
                    {
                        _isVariable = true;
                        if (_stringBuilder.Length > 0)
                        {
                            entries.Add(new Entry(EntryType.Text, _stringBuilder.ToString()));
                            _stringBuilder.Clear();
                        }
                        
                        continue;
                    }
                }
                else if (symbolChar == '}')
                {
                    if (_isVariable)
                    {
                        _isVariable = false;
                        if (_stringBuilder.Length > 0)
                        {
                            entries.Add(new Entry(EntryType.Keyword, _stringBuilder.ToString()));
                            _stringBuilder.Clear();
                        }

                        continue;
                    }
                }

                if (symbol == -1)
                {
                    if (_stringBuilder.Length > 0)
                    {
                        entries.Add(new Entry(
                            _isVariable ? EntryType.Keyword : EntryType.Text,
                            _stringBuilder.ToString()));
                    }

                    break;
                }
                else
                {
                    _stringBuilder.Append(symbolChar);
                }
            }

            return entries;
        }
    }
}
