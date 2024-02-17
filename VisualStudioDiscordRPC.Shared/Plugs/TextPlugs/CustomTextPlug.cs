using VisualStudioDiscordRPC.Shared.Services;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class CustomTextPlug : BaseTextPlug
    {
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        public string Pattern => _pattern;

        private readonly string _id;
        private string _name;

        private string _pattern;
        private StringObserver _stringObserver;

        private readonly VariableService _variableService = ServiceRepository.Default.GetService<VariableService>();

        public CustomTextPlug(string id, string name)
        {
            _id = id;
            _name = name;
        }

        public void SetPattern(string pattern)
        {
            if (_stringObserver == null)
            {
                _stringObserver = new StringObserver();
            }

            _stringObserver.Clear();

            var parser = new ObservableStringParser();
            var entries = parser.Parse(pattern);

            foreach (var entry in entries)
            {
                switch (entry.Type)
                {
                    case ObservableStringParser.EntryType.Text:
                        _stringObserver.AddText(entry.Value);
                        break;

                    case ObservableStringParser.EntryType.Keyword:
                        var variable = _variableService.GetVariableByName(entry.Value);
                        if (variable != null)
                            _stringObserver.AddText(new ObservableVariable(variable));
                        break;
                }
            }

            _pattern = pattern;
        }

        public void ClearObserver()
        {
            _stringObserver?.Clear();
        }

        public override void Enable()
        {
            _stringObserver.Changed += OnStringChanged;
        }

        public override void Disable()
        {
            _stringObserver.Changed -= OnStringChanged;
        }

        public override string GetId()
        {
            return _id;
        }

        protected override string GetData()
        {
            return _stringObserver.ToString();
        }

        private void OnStringChanged()
        {
            Update();
        }
    }
}
