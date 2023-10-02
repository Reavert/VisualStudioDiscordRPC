using System;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public abstract class Variable
    {
        public event EventHandler<string> Changed;

        public abstract string GetData();

        protected void RaiseChangedEvent()
        {
            Changed?.Invoke(this, GetData());
        }
    }
}
