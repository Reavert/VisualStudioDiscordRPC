using System;
using VisualStudioDiscordRPC.Shared.Plugs;

namespace VisualStudioDiscordRPC.Shared.Nests.Base
{
    public abstract class BaseNest
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
