using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseDataUpdater<TData> : BaseUpdater
    {
        private BaseDataSlot<TData> _installedSlot;

        public override BaseSlot BaseSlot
        {
            get => Slot;
            set => Slot = (BaseDataSlot<TData>)value;
        }
        
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
            }
        }

        protected abstract void Update(TData data);
    }
}
