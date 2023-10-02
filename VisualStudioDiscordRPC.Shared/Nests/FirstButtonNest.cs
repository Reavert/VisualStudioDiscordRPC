using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class FirstButtonNest : BaseButtonNest
    {
        public FirstButtonNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(ButtonInfo data)
        {
            SetButton(0, data);
        }
    }
}
