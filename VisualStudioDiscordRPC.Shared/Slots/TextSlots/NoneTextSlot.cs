namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class NoneTextSlot : TextSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override string GetData()
        {
            return null;
        }
    }
}
