using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class BaseDataSlot<T> : BaseSlot
    {
        public event Action<T> UpdatePerformed;

        public override void Update()
        {
            UpdatePerformed?.Invoke(GetData());
        }

        protected abstract T GetData();
    }
}
