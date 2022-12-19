using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal class DetailsUpdater : BaseDiscordRpcUpdater
    {
        public DetailsUpdater(DiscordRpcClient client) : base(client)
        { }

        protected override void OnSlotUpdatePerformed(string data)
        {
            DiscordRpcClient.UpdateDetails(data);
        }
    }
}
