using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class StateNest : BaseDiscordRpcNest<string>
    {
        public StateNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.State = data;
        }
    }
}
