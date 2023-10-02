using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Nests.Base
{
    public abstract class BaseDiscordRpcNest<T> : BaseDataNest<T>
    {
        protected readonly RichPresence RichPresence;

        protected BaseDiscordRpcNest(RichPresence richPresence)
        {
            RichPresence = richPresence;
        }
    }
}
