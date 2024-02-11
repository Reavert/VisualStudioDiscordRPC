using DiscordRPC;
using System.Text;
using VisualStudioDiscordRPC.Shared.Nests.Base;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class StateNest : BaseDiscordRpcNest<string>
    {
        public StateNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(string data)
        {
            RichPresence.State = StringHelper.ReEncodeWithMaxLength(data, 128, Encoding.UTF8);
        }
    }
}
