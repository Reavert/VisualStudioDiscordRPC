using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public abstract class TimerSlot : BaseDataSlot<Timestamps>
    {
        private Timestamps _changeTimestamp;
        protected Timestamps ChangeTimestamp 
        {
            get => _changeTimestamp;
            set
            {
                _changeTimestamp = value;
                Update();
            }
        }

        protected TimerSlot()
        {
            ChangeTimestamp = Timestamps.Now;
        }

        protected sealed override Timestamps GetData()
        {
            return ChangeTimestamp;
        }
    }
}
