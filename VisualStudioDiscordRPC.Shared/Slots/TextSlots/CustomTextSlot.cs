using VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class CustomTextSlot : TextSlot
    {
        private readonly StringObserver _customStringObserver;

        public CustomTextSlot(StringObserver customStringObserver)
        {
            _customStringObserver = customStringObserver;
        }

        public override void Enable()
        {
            _customStringObserver.Changed += OnStringChanged;
        }

        public override void Disable()
        {
            _customStringObserver.Changed -= OnStringChanged;
        }

        protected override string GetData()
        {
            return _customStringObserver.ToString();
        }

        private void OnStringChanged()
        {
            Update();
        }
    }
}
