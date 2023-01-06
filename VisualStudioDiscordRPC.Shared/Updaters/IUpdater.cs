using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public interface IUpdater<T>
    {
        AbstractSlot<T> Slot { get; set; }
    }
}
