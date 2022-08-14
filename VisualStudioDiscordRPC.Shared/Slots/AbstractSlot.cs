using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public abstract class AbstractSlot : ISlot
    {
        protected IObserver Observer;
        protected SlotUpdateHandler SlotUpdateHandler;

        public AbstractSlot(IObserver observer, SlotUpdateHandler slotUpdateHandler)
        {
            Observer = observer;
            SlotUpdateHandler = slotUpdateHandler;
        }

        public abstract void Enable();

        public abstract void Disable();
    }
}
