using Microsoft.VisualStudio.Text.Editor;
using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class SlotUpdateHandler
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

        private Action<string> _onUpdateAction;

        public SlotUpdateHandler(Action<string> onUpdateAction)
        {
            _onUpdateAction = onUpdateAction;
        }

        private void OnSlotUpdatePerformed(string data)
        {
            _onUpdateAction?.Invoke(data);
        }

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
    }
}
