using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public abstract class TimerSlot : AbstractSlot<Timestamps>
    {
        protected override Timestamps GetData()
        {
            return Timestamps.Now;
        }
    }
}
