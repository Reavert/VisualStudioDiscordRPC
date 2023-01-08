using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters.Base
{
    public abstract class BaseUpdater<TData> : IUpdater<TData>
    {
        public bool Enabled { get; set; }

        private AbstractSlot<TData> _installedSlot;
        public AbstractSlot<TData> Slot
        {
            get => _installedSlot;
            set
            {
                ClearSlotSubscription();

                _installedSlot = value;

                SetSlotSubscription();
            }
        }

        private void SetSlotSubscription()
        {
            if (_installedSlot != null)
            {
                _installedSlot.UpdatePerformed += OnSlotUpdatePerformed;
            }
        }

        private void ClearSlotSubscription()
        {
            if (_installedSlot != null)
            {
                _installedSlot.UpdatePerformed -= OnSlotUpdatePerformed;
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
