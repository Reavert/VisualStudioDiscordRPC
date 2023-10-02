using System;
using VisualStudioDiscordRPC.Shared.Plugs;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseUpdater
    {
        public event Action Changed;

        public bool Enabled { get; set; }

        public abstract BasePlug BasePlug { get; set; }

        protected void RaiseOnChangedEvent()
        {
            Changed?.Invoke();
        }
    }
}
