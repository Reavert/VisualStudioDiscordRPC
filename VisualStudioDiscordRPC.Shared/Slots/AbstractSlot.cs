using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot<T> : ISlot<T>
    {
        public event Action<T> UpdatePerformed;

        public void Update()
        {
            UpdatePerformed?.Invoke(GetData());
        }

        public abstract void Enable();

        public abstract void Disable();

        protected abstract T GetData();
    }
}
