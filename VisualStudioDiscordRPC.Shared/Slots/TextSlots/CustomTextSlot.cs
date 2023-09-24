using VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class CustomTextSlot : TextSlot
    {
        public string Name => _name;
        
        private readonly StringObserver _customStringObserver;
        private readonly string _name;

        public CustomTextSlot(string name, StringObserver customStringObserver)
        {
            _name = name;
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
