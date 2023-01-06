using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class DetailsUpdater : BaseDiscordRpcUpdater<string>
    {
        public DetailsUpdater(DiscordRpcClient client) : base(client)
        { }

        protected override void OnSlotUpdatePerformed(string data)
        {
            DiscordRpcClient.UpdateDetails(data);
        }
    }
}
