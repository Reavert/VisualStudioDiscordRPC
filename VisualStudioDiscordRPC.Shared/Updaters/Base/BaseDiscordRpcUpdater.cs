using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseDiscordRpcUpdater<T> : BaseUpdater<T>
    {
        protected RichPresence RichPresence;

        protected BaseDiscordRpcUpdater(RichPresence richPresence)
        {
            RichPresence = richPresence;
        }
    }
}
