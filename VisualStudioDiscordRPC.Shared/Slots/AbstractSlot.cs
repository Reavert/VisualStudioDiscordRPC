using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot<T> : ISlot<T>
    {
        public event Action<T> UpdatePerformed;

        protected void PerformUpdate(T data)
        {
            UpdatePerformed?.Invoke(data);
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}
