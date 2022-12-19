using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal class LargeIconUpdater : BaseDiscordRpcUpdater
    {
        public LargeIconUpdater(DiscordRpcClient client) : base(client) { }

        protected override void OnSlotUpdatePerformed(string data)
        {
            DiscordRpcClient.UpdateLargeAsset(data);
        }
    }
}
