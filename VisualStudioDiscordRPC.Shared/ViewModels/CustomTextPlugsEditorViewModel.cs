using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomTextPlugsEditorViewModel : ViewModelBase
    {
        public ObservableCollection<CustomTextPlugData> CustomTextPlugs { get; set; }
        public IReadOnlyCollection<VariableDescriptor> Variables => _variableService.GetVariables();
        public CustomTextPlugData SelectedItem { get; set; }

        public RelayCommand NewCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private readonly PlugService _plugService;
        private readonly VariableService _variableService;

        public CustomTextPlugsEditorViewModel(IEnumerable<CustomTextPlugData> data) 
        {
            _plugService = ServiceRepository.Default.GetService<PlugService>();
            _variableService = ServiceRepository.Default.GetService<VariableService>();

            CustomTextPlugs = new ObservableCollection<CustomTextPlugData>(data);

            NewCommand = new RelayCommand(OnNewCommand);
            DeleteCommand = new RelayCommand(OnDeleteCommand);
        }

        private void OnNewCommand(object parameter)
        {
            CustomTextPlugs.Add(new CustomTextPlugData(_plugService.GenerateUniqueCustomTextPlugId(), "New", string.Empty));
        }

        private void OnDeleteCommand(object parameter)
        {
            if (SelectedItem == null)
                return;

            CustomTextPlugs.Remove(SelectedItem);
        }
    }
}
