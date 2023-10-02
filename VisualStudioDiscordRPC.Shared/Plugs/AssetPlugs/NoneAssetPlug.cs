using VisualStudioDiscordRPC.Shared.Data;

namespace VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs
{
    public class NoneAssetPlug : BaseAssetPlug
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
