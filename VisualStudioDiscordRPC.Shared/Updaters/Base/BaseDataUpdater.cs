using VisualStudioDiscordRPC.Shared.Plugs;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseDataUpdater<TData> : BaseUpdater
    {
        private BaseDataPlug<TData> _installedPlug;

        public override BasePlug BasePlug
        {
            get => Plug;
            set => Plug = (BaseDataPlug<TData>)value;
        }

        public BaseDataPlug<TData> Plug
        {
            get => _installedPlug;
            set
            {
                if (_installedPlug != null)
                    _installedPlug.UpdatePerformed -= OnPlugUpdatePerformed;

                _installedPlug = value;

                if (_installedPlug != null)
                    _installedPlug.UpdatePerformed += OnPlugUpdatePerformed;
            }
        }

        private void OnPlugUpdatePerformed(TData data)
        {
            if (Enabled)
            {
                Update(data);
                RaiseOnChangedEvent();
            }
        }

        protected abstract void Update(TData data);
    }
}
