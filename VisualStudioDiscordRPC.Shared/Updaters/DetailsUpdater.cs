using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class DetailsUpdater : BaseDiscordRpcUpdater<string>
    {
        public DetailsUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.Details = data;
        }
    }
}
