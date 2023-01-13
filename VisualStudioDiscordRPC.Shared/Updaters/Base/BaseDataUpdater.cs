using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseDataUpdater<TData> : BaseUpdater
    {
        public override BaseSlot BaseSlot
        {
            get => Slot;
            set => Slot = (BaseDataSlot<TData>)value;
        }

        private BaseDataSlot<TData> _installedSlot;

        public BaseDataSlot<TData> Slot
        {
            get => _installedSlot;
            set
            {
                if (_installedSlot != null)
                    _installedSlot.UpdatePerformed -= OnSlotUpdatePerformed;

                _installedSlot = value;

                if (_installedSlot != null)
                    _installedSlot.UpdatePerformed += OnSlotUpdatePerformed;
            }
        }

        private void OnSlotUpdatePerformed(TData data)
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
