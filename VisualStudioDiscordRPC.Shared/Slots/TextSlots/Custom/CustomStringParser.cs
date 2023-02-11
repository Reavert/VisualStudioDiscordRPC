using System.IO;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public static class CustomStringParser
    {
        private const string FileName = "filename";

        public static CustomString Parse(string text)
        {
            var customString = new CustomString();

            var stringBuilder = new StringBuilder();
            var reader = new StringReader(text);

            var parserState = ParserState.ReadStaticText;

            char symbol;
            while (true)
            {
                int symbolInt = reader.Read();
                if (symbolInt == -1)
                {
                    break;
                }

                symbol = (char)symbolInt;
                switch (parserState)
                {
                    case ParserState.ReadStaticText:
                        if (symbol == '{')
                        {
                            customString.AddText(stringBuilder.ToString());
                            stringBuilder.Clear();

                            parserState = ParserState.ReadVariable;
                        }
                        else
                        {
                            stringBuilder.Append(symbol);
                        }

                        break;

                    case ParserState.ReadVariable:
                        if (symbol == '}')
                        {
                            string variableName = stringBuilder.ToString();
                            ITextSource textSource = 
                                CustomTextSources.GetGlobalTextSourceByName(variableName);

                            customString.AddText(textSource);
                            stringBuilder.Clear();

                            parserState = ParserState.ReadStaticText;
                        }
                        else
                        {
                            stringBuilder.Append(symbol);
                        }

                        break;
                }
            }

            return customString;
        }

        private enum ParserState
        {
            ReadStaticText,
            ReadVariable
        }
    }
}
