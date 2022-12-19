using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal interface IUpdater
    {
        ISlot Slot { get; set; }
    }
}
