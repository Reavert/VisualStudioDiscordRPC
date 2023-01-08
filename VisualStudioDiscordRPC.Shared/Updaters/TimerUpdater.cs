using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class TimerUpdater : BaseDiscordRpcUpdater<Timestamps>
    {
        public TimerUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(Timestamps data)
        {
            RichPresence.Timestamps = data;
        }
    }
}
