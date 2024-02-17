using DiscordRPC;
using System.Text;
using VisualStudioDiscordRPC.Shared.Nests.Base;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class DetailsNest : BaseDiscordRpcNest<string>
    {
        public DetailsNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.Details = StringHelper.RecodeToFitMaxLength(data, 128, Encoding.UTF8);
        }
    }
}
