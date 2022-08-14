using System;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class SlotUpdateHandler
    {
        private Action<string> _onUpdateAction;

        public SlotUpdateHandler(Action<string> onUpdateAction)
        {
            _onUpdateAction = onUpdateAction;
        }

        public void Update(string data)
        {
            _onUpdateAction.Invoke(data);
        }
    }
}
