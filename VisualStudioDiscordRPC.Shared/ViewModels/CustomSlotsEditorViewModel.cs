using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.ViewModels.CustomSlots;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class CustomSlotsEditorViewModel : ViewModelBase
    {
        private ICustomSlotViewModel _customSlotViewModel;
        public ICustomSlotViewModel CustomSlotViewModel
        {
            get => _customSlotViewModel;
            set => SetProperty(ref _customSlotViewModel, value);
        }

        private readonly ObservableCollection<object> _customSlots;
        public ObservableCollection<object> CustomSlots => _customSlots;

        public object SelectedSlot { get; set; }

        private readonly RelayCommand _addCustomSlotCommand;
        public RelayCommand AddCustomSlotCommand => _addCustomSlotCommand;

        private readonly RelayCommand _removeSlotCommand;
        public RelayCommand RemoveSlotCommand => _removeSlotCommand;

        public CustomSlotsEditorViewModel(List<object> customSlots, ICustomSlotViewModel customEditorViewModel) 
        {
            _customSlots = new ObservableCollection<object>(customSlots);
            _addCustomSlotCommand = new RelayCommand(AddCustomSlot);
            _removeSlotCommand = new RelayCommand(RemoveCustomSlot);

            CustomSlotViewModel = customEditorViewModel;
        }

        public void AddCustomSlot(object parameter)
        {
            _customSlots.Add(_customSlotViewModel.GetSlotSetting());
            _customSlotViewModel.ClearView();
        }

        public void RemoveCustomSlot(object parameter)
        {
            if (SelectedSlot == null)
            {
                return;
            }    

            _customSlots.Remove(SelectedSlot);
        }
    }
}
