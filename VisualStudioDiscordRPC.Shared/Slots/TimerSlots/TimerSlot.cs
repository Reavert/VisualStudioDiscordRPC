using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public abstract class TimerSlot : BaseDataSlot<Timestamps>
    {
        protected override Timestamps GetData()
        {
            return Timestamps.Now;
        }
    }
}
