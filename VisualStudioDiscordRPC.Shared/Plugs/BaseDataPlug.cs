using System;

namespace VisualStudioDiscordRPC.Shared.Plugs
{
    public abstract class BaseDataPlug<T> : BasePlug
    {
        public event Action<T> UpdatePerformed;

        public override void Update()
        {
            UpdatePerformed?.Invoke(GetData());
        }

        protected abstract T GetData();
    }
}
