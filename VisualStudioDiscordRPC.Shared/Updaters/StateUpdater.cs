using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class StateUpdater : BaseDiscordRpcUpdater<string>
    {
        public StateUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.State = data;
        }
    }
}
