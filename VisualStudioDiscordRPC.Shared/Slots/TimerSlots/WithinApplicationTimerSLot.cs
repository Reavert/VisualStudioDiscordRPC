using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinApplicationTimerSlot : TimerSlot
    {
        public WithinApplicationTimerSlot() : base()
        {
            // Never change timestamp for application scope.
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
