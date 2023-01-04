using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal abstract class BaseDiscordRpcUpdater<T> : BaseUpdater<T>
    {
        protected DiscordRpcClient DiscordRpcClient;

        protected BaseDiscordRpcUpdater(DiscordRpcClient client)
        {
            DiscordRpcClient = client;
        }
    }
}
