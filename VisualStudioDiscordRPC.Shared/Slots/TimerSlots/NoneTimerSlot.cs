using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class NoneTimerSlot : TimerSlot
    {
        public NoneTimerSlot() 
        {
            ClearTimestamp();
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
