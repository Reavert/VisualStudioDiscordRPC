using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class ListedSettingEditorViewModel : ViewModelBase
    {
        private string _settingName;
        public string SettingName => _settingName;

        public List<string> Items => _items;

        public string SelectedItem { get; set; }

        private string _newSettingValue;
        public string NewSettingValue 
        {
            get => _newSettingValue;
            set => SetProperty(ref _newSettingValue, value);
        }

        private readonly RelayCommand _addCommand;
        public RelayCommand AddCommand => _addCommand;

        private readonly RelayCommand _removeCommand;
        public RelayCommand RemoveCommand => _removeCommand;

        private readonly SettingsService _settingsService;
        private readonly List<string> _items;

        public ListedSettingEditorViewModel(string settingName) 
        {
            _settingsService = ServiceRepository.Default.GetService<SettingsService>();

            _addCommand = new RelayCommand(AddItem);
            _removeCommand = new RelayCommand(RemoveItem);
            
            _settingName = settingName;
            _items = _settingsService.ReadList<string>(settingName);

            _settingsService.Set(settingName, _items);
        }

        private void AddItem(object parameter)
        {
            if (string.IsNullOrEmpty(NewSettingValue))
            {
                return;
            }

            _items.Add(NewSettingValue);
            OnPropertyChanged(nameof(Items));

            NewSettingValue = string.Empty;
        }

        private void RemoveItem(object parameter)
        {
            if (SelectedItem == null)
            {
                return;
            }

            _items.Remove(SelectedItem);
            OnPropertyChanged(nameof(Items));
        }
    }
}
