using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class DetailsNest : BaseDiscordRpcNest<string>
    {
        public DetailsNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.Details = data;
        }
    }
}
