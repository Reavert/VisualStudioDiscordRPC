using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomTextPlugViewModel : ViewModelBase
    {
        public string Id => _model.GetId();
        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Pattern
        {
            get => _model.Pattern;
            set
            {
                _model.SetPattern(value);
                OnPropertyChanged(nameof(Pattern));
            }
        }

        private readonly CustomTextPlug _model;

        public CustomTextPlugViewModel(CustomTextPlug model)
        {
            _model = model;
        }
    }
}
