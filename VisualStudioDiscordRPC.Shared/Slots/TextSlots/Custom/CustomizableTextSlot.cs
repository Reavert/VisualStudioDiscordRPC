namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class CustomizableTextSlot : TextSlot
    {
        private CustomString _customString;

        public CustomizableTextSlot(string customString)
        {
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
