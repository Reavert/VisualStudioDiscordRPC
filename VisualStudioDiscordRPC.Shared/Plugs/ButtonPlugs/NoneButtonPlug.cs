using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs
{
    public class NoneButtonPlug : BaseButtonPlug
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override ButtonInfo GetData()
        {
            return ButtonInfo.None;
        }
    }
}
