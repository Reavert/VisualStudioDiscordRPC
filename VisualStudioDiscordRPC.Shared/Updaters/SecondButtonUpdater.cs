using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class SecondButtonUpdater : BaseButtonUpdater
    {
        public SecondButtonUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(ButtonInfo data)
        {
            SetButton(1, data);
        }
    }
}
