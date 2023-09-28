using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomTextSlotsEditorViewModel : ViewModelBase
    {
        public ObservableCollection<CustomTextSlotData> CustomTextSlots { get; set; }
        public IReadOnlyCollection<VariableDescriptor> Variables => _variableService.GetVariables();
        public CustomTextSlotData SelectedItem { get; set; }

        public RelayCommand NewCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private readonly SlotService _slotService;
        private readonly VariableService _variableService;

        public CustomTextSlotsEditorViewModel(IEnumerable<CustomTextSlotData> data) 
        {
            _slotService = ServiceRepository.Default.GetService<SlotService>();
            _variableService = ServiceRepository.Default.GetService<VariableService>();

            CustomTextSlots = new ObservableCollection<CustomTextSlotData>(data);

            NewCommand = new RelayCommand(OnNewCommand);
            DeleteCommand = new RelayCommand(OnDeleteCommand);
        }

        private void OnNewCommand(object parameter)
        {
            CustomTextSlots.Add(new CustomTextSlotData(_slotService.GenerateUniqueCustomTextSlotId(), "New", string.Empty));
        }

        private void OnDeleteCommand(object parameter)
        {
            if (SelectedItem == null)
                return;

            CustomTextSlots.Remove(SelectedItem);
        }
    }
}
