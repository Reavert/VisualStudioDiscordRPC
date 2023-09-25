namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class CustomTextSlotData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pattern { get; set; }

        public CustomTextSlotData(string id, string name, string pattern)
        {
            Id = id;
            Name = name;
            Pattern = pattern;
        }
    }
}
