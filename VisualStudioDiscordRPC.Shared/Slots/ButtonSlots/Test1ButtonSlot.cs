using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Slots.ButtonSlots
{
    public class Test1ButtonSlot : ButtonSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override ButtonInfo GetData()
        {
            return new ButtonInfo("ABC", "https://github.com/Ryavell");
        }
    }
}
