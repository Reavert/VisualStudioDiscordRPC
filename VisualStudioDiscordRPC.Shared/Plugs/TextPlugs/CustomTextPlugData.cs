namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class CustomTextPlugData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }

        public CustomTextPlugData(string id, string name, string pattern)
        {
            Id = id;
            Name = name;
            Pattern = pattern;
        }
    }
}
