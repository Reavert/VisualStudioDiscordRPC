using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinApplicationTimerSLot : TimerSlot
    {
        public WithinApplicationTimerSLot() : base()
        {
            // Never change timestamp for application scope.
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
