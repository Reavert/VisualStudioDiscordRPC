using System;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public interface IObservableString
    {
        event Action Changed;
        string Text { get; }
    }
}
