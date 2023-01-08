using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public interface IUpdater<T>
    {
        AbstractSlot<T> Slot { get; set; }
    }
}
