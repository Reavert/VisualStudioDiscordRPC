using DiscordRPC;

namespace VisualStudioDiscordRPC.Shared.Updaters
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
