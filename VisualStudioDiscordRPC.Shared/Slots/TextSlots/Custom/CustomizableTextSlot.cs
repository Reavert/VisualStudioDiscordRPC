namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class CustomizableTextSlot : TextSlot, ICustomSlot
    {
        private readonly CustomString _customString;
        private readonly string _rawCustomString;

        public string Name => $"Custom: {_rawCustomString}";

        public CustomizableTextSlot(string customString)
        {
            _rawCustomString = customString;
            _customString = CustomStringParser.Parse(customString);
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
