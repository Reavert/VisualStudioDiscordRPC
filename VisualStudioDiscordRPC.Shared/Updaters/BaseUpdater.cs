using System;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    internal abstract class BaseUpdater : IUpdater
    {
        private ISlot _installedSlot;
        public ISlot Slot
        {
            set
            {
                ClearSlotSubscription();

                _installedSlot = value;

                SetSlotSubscription();
            }
            get => _installedSlot;
        }

        public bool IsInstalled => _installedSlot != null;

        private void SetSlotSubscription()
        {
            if (IsInstalled)
            {
                _installedSlot.UpdatePerformed += OnSlotUpdatePerformed;
            }
        }

        private void ClearSlotSubscription()
        {
            if (IsInstalled)
            {
                _installedSlot.UpdatePerformed -= OnSlotUpdatePerformed;
            }
        }

        protected abstract void OnSlotUpdatePerformed(string data);
    }
}
