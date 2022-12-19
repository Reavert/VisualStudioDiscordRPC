using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal class StateUpdater : BaseDiscordRpcUpdater
    {
        public StateUpdater(DiscordRpcClient client) : base(client)
        { }

        protected override void OnSlotUpdatePerformed(string data)
        {
            DiscordRpcClient.UpdateState(data);
        }
    }
}
