using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomTextSlotsEditorViewModel : ViewModelBase
    {
        public ObservableCollection<CustomTextSlotData> CustomTextSlots { get; set; }
        public IReadOnlyCollection<MacroData> MacroDatas => _macroService.GetMacroDatas();
        public CustomTextSlotData SelectedItem { get; set; }

        public RelayCommand NewCommand { get; }
        public RelayCommand DeleteCommand { get; }

        private readonly SlotService _slotService;
        private readonly MacroService _macroService;

        public CustomTextSlotsEditorViewModel(IEnumerable<CustomTextSlotData> data) 
        {
            _slotService = ServiceRepository.Default.GetService<SlotService>();
            _macroService = ServiceRepository.Default.GetService<MacroService>();

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
