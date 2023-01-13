using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class FirstButtonUpdater : BaseButtonUpdater
    {
        public FirstButtonUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(ButtonInfo data)
        {
            SetButton(0, data);
        }
    }
}
