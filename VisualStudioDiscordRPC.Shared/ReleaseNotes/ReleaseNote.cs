namespace VisualStudioDiscordRPC.Shared.ReleaseNotes
{
    public class ReleaseNote
    {
        public readonly string Version;
        public readonly string[] Notes;

        public ReleaseNote(string version, string[] notes)
        {
            Version = version;
            Notes = notes;
        }
    }
}
