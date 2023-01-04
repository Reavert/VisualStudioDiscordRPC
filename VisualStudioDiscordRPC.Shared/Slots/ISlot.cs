using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public interface ISlot<T>
    {
        event Action<T> UpdatePerformed;
        void Enable();
        void Disable();
    }
}
