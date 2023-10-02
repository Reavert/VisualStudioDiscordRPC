using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class TimerNest : BaseDiscordRpcNest<Timestamps>
    {
        public TimerNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(Timestamps data)
        {
            RichPresence.Timestamps = data;
        }
    }
}
