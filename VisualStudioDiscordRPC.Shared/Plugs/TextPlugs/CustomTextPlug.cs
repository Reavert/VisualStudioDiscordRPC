namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class CustomTextPlug : BaseTextPlug
    {
        public string Name => _name;
        
        private readonly string _id;
        private readonly string _name;
        private readonly StringObserver _customStringObserver;

        public CustomTextPlug(string id, string name, StringObserver customStringObserver)
        {
            _id = id;
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

        public override string GetId()
        {
            return _id;
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
