using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Slots.AssetSlots
{
    public class NoneAssetSlot : AssetSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override AssetInfo GetData()
        {
            return AssetInfo.None;
        }
    }
}
