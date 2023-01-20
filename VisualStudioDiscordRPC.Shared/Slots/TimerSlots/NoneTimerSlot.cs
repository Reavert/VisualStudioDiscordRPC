using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class NoneTimerSlot : TimerSlot
    {
        public NoneTimerSlot() 
        {
            ChangeTimestamp = null;
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
