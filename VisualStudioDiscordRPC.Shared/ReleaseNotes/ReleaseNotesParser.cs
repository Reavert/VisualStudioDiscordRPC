using System.Collections.Generic;
using System.IO;

namespace VisualStudioDiscordRPC.Shared.ReleaseNotes
{
    public class ReleaseNotesParser
    {
        private StringReader _reader;

        public ReleaseNotesParser(string releaseNotesText)
        {
            _reader = new StringReader(releaseNotesText);
        }

        public bool ReadReleaseNote(out ReleaseNote releaseNote)
        {
            releaseNote = null;

            string version = _reader.ReadLine();
            if (string.IsNullOrEmpty(version))
            {
                return false;
            }

            var notes = new List<string>();
            string currentLine;

            do
            {
                currentLine = _reader.ReadLine();
                if (currentLine == null)
                {
                    break;
                }

                if (currentLine.StartsWith("- "))
                {
                    notes.Add(currentLine.Substring(2).Trim());
                }
            } while (!string.IsNullOrEmpty(currentLine));

            releaseNote = new ReleaseNote(version, notes.ToArray());
            return true;
        }
    }
}
