using System;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot : ISlot
    {
        protected IObserver Observer;
        public event Action<string> UpdatePerformed;

        public AbstractSlot(IObserver observer)
        {
            Observer = observer;
        }

        protected void PerformUpdate(string data)
        {
            UpdatePerformed?.Invoke(data);
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}
