using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal interface IUpdater<T>
    {
        AbstractSlot<T> Slot { get; set; }
    }
}
