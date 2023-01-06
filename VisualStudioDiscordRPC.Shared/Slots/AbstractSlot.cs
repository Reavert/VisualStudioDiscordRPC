using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot<T> : ISlot<T>
    {
        public event Action<T> UpdatePerformed;

        private T _lastData;

        protected void PerformUpdate(T data)
        {
            _lastData = data;
            UpdatePerformed?.Invoke(data);
        }

        public void UpdateWithLastData()
        {
            UpdatePerformed?.Invoke(_lastData);
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}
