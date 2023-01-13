using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Slots.ButtonSlots
{
    public class Test2ButtonSlot : ButtonSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override ButtonInfo GetData()
        {
            return new ButtonInfo("Not working", "https://roh-online.com");
        }
    }
}
