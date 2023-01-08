using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class NoneTimerSlot : TimerSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override Timestamps GetData()
        {
            return null;
        }
    }
}
