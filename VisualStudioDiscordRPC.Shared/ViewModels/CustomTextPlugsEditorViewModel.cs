using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Services;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomTextPlugsEditorViewModel : ViewModelBase
    {
        public IReadOnlyCollection<CustomTextPlugViewModel> CustomTextPlugs => 
            _plugService.GetPlugsOfType<CustomTextPlug>().Select(plug => new CustomTextPlugViewModel(plug)).ToArray();

        public IReadOnlyCollection<VariableDescriptor> Variables => _variableService.GetVariables();
        public CustomTextPlugViewModel SelectedItem { get; set; }

        public RelayCommand NewCommand { get; }
        public RelayCommand ApplyCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private readonly PlugService _plugService;
        private readonly VariableService _variableService;

        public CustomTextPlugsEditorViewModel() 
        {
            _plugService = ServiceRepository.Default.GetService<PlugService>();
            _variableService = ServiceRepository.Default.GetService<VariableService>();

            NewCommand = new RelayCommand(OnNewCommand);
            DeleteCommand = new RelayCommand(OnDeleteCommand);
        }

        private void OnNewCommand(object parameter)
        {
            _plugService.CreateCustomTextPlug("New plug", string.Empty);
            OnPropertyChanged(nameof(CustomTextPlugs));
        }

        private void OnDeleteCommand(object parameter)
        {
            if (SelectedItem == null)
                return;

            _plugService.DeleteCustomTextPlug(SelectedItem.Id);
            OnPropertyChanged(nameof(CustomTextPlugs));
        }
    }
}
