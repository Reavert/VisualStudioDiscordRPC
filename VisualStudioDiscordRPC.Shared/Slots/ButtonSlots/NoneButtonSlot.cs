using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Slots.ButtonSlots
{
    public class NoneButtonSlot : ButtonSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override ButtonInfo GetData()
        {
            return ButtonInfo.None;
        }
    }
}
