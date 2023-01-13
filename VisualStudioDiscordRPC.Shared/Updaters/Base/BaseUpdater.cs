using System;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseUpdater
    {
        public event Action Changed;

        public bool Enabled { get; set; }

        public abstract BaseSlot BaseSlot { get; set; }

        protected void RaiseOnChangedEvent()
        {
            Changed?.Invoke();
        }
    }
}
