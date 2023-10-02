using System;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public interface IObservableString
    {
        event Action Changed;
        string Text { get; }
    }
}
