using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class SmallIconUpdater : BaseDiscordRpcUpdater<AssetInfo>
    {
        public SmallIconUpdater(DiscordRpcClient client) : base(client)
        { }

        protected override void OnSlotUpdatePerformed(AssetInfo data)
        {
            DiscordRpcClient.UpdateSmallAsset(data.Key, data.Description);
        }
    }
}
