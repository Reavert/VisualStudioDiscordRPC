using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseDiscordRpcUpdater<T> : BaseDataUpdater<T>
    {
        protected RichPresence RichPresence;

        protected BaseDiscordRpcUpdater(RichPresence richPresence)
        {
            RichPresence = richPresence;
        }
    }
}
