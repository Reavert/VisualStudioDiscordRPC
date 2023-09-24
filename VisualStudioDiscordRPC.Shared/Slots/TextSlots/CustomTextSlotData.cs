namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class CustomTextSlotData
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Pattern;

        public CustomTextSlotData(string id, string name, string pattern)
        {
            Id = id;
            Name = name;
            Pattern = pattern;
        }
    }
}
