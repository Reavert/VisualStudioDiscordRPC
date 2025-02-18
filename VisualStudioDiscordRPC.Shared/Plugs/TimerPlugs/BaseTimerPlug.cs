using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public abstract class BaseTimerPlug : BaseDataPlug<Timestamps>
    {
        private Timestamps _changeTimestamp;

        protected BaseTimerPlug()
        {
            SyncTimestamp();
        }

        public void SyncTimestamp()
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
