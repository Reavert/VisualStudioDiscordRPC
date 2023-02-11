using System;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public interface ITextSource
    {
        string Name { get; }
        string Text { get; }

        event Action Changed;
    }
}
