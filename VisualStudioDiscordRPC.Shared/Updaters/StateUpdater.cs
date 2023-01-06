using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class StateUpdater : BaseDiscordRpcUpdater<string>
    {
        public StateUpdater(DiscordRpcClient client) : base(client)
        { }

        protected override void OnSlotUpdatePerformed(string data)
        {
            DiscordRpcClient.UpdateState(data);
        }
    }
}
