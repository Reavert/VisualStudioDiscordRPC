using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public abstract class BaseUpdater<TData> : IUpdater<TData>
    {
        private AbstractSlot<TData> _installedSlot;
        public AbstractSlot<TData> Slot
        {
            set
            {
                ClearSlotSubscription();

                _installedSlot = value;

                SetSlotSubscription();
            }
            get => _installedSlot;
        }

        public bool IsSlotInstalled => _installedSlot != null;

        private void SetSlotSubscription()
        {
            if (IsSlotInstalled)
            {
                _installedSlot.UpdatePerformed += OnSlotUpdatePerformed;
            }
        }

        private void ClearSlotSubscription()
        {
            if (IsSlotInstalled)
            {
                _installedSlot.UpdatePerformed -= OnSlotUpdatePerformed;
            }
        }

        protected abstract void OnSlotUpdatePerformed(TData data);
    }
}
