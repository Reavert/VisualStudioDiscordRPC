using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class LargeIconUpdater : BaseDiscordRpcUpdater<AssetInfo>
    {
        public LargeIconUpdater(DiscordRpcClient client) : base(client) 
        { }

        protected override void OnSlotUpdatePerformed(AssetInfo data)
        {
            DiscordRpcClient.UpdateLargeAsset(data.Key, data.Description);
        }
    }
}
