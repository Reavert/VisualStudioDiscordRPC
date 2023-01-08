namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class NoneTextSlot : TextSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override string GetData()
        {
            return string.Empty;
        }
    }
}
