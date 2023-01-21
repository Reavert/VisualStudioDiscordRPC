using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinApplicationTimerSLot : TimerSlot
    {
        public WithinApplicationTimerSLot() 
        {
            ChangeTimestamp = Timestamps.Now;
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
