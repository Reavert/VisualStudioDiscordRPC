using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public interface ISlot
    {
        event Action<string> UpdatePerformed;
        void Enable();
        void Disable();
    }
}
