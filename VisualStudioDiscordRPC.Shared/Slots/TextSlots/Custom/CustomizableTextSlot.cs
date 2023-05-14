namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class CustomizableTextSlot : TextSlot, ICustomSlot
    {
        private readonly CustomString _customString;

        public string Name => $"Custom";

        public CustomizableTextSlot(CustomString customString)
        {
            _customString = customString;
        }

        public override void Enable()
        {
            _customString.Changed += OnCustomStringChanged;
        }

        public override void Disable()
        {
            _customString.Changed -= OnCustomStringChanged;
        }

        private void OnCustomStringChanged()
        {
            Update();
        }

        protected override string GetData()
        {
            return _customString.ToString();
        }
    }
}
