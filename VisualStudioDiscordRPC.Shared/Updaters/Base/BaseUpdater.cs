using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseUpdater
    {
        public bool Enabled { get; set; }

        public abstract BaseSlot BaseSlot { get; set; }
    }
}
