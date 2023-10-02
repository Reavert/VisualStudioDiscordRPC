using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class SecondButtonNest : BaseButtonNest
    {
        public SecondButtonNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(ButtonInfo data)
        {
            SetButton(1, data);
        }
    }
}
