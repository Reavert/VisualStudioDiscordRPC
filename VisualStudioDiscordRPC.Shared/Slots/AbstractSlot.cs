using System;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot : ISlot
    {
        public event Action<string> UpdatePerformed;

        protected void PerformUpdate(string data)
        {
            UpdatePerformed?.Invoke(data);
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}
