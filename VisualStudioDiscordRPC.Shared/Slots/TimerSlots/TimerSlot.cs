using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public abstract class TimerSlot : BaseDataSlot<Timestamps>
    {
        private Timestamps _changeTimestamp;

        protected TimerSlot()
        {
            SyncTimestamp();
        }

        protected void SyncTimestamp()
        {
            _changeTimestamp = Timestamps.Now;
            Update();
        }

        protected void ClearTimestamp()
        {
            _changeTimestamp = null;
            Update();
        }

        protected sealed override Timestamps GetData()
        {
            return _changeTimestamp;
        }
    }
}
