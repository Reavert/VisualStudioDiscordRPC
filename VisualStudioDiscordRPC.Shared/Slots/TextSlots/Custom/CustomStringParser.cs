using System.Drawing.Text;
using System.IO;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class CustomStringParser
    {
        private const char StartVariableLex = '{';
        private const char EndVariableLex = '}';

        private ParserState _parserState = ParserState.Initial;
        private StringBuilder _stringBuilder = new StringBuilder();

        private StringReader _stringReader;
        private char _currentSymbol;

        public CustomString Parse(string text)
        {
            _parserState = ParserState.Initial;
            _stringReader = new StringReader(text);

            while (_parserState != ParserState.End)
            {
                UpdateState();
            }
        }

        private bool TryRead(out char symbol)
        {
            int symbolInt = _stringReader.Read();
            if (symbolInt == -1)
            {
                symbol = '\0';
                return false;
            }

            symbol = (char)symbolInt;
            return true;
        }

        private void UpdateState()
        {
            switch (_parserState)
            {
                case ParserState.Initial:
                    if (TryRead(out _currentSymbol))
                    {
                        _parserState = _currentSymbol == StartVariableLex 
                            ? ParserState.ReadVariable 
                            : ParserState.ReadText;
                    }
                    else
                    {
                        _parserState = ParserState.End;
                    }

                    break;

                case ParserState.ReadText:
                    if (TryRead(out _currentSymbol))
                    {
                        if (_currentSymbol == StartVariableLex)
                        {
                            _parserState = ParserState.ReadVariable;
                            break;
                        }


                    }
                    else
                    {
                        _parserState = ParserState.End;
                    }



                    break;

            }
        }

        private enum ParserState
        {
            Initial,

            ReadText,
            ReadVariable,

            End
        }
    }
}
