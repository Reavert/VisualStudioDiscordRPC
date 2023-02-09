using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class ListedSettingEditorViewModel : ViewModelBase
    {
        private string _settingName;
        public string SettingName => _settingName;

        public List<string> Items
        { 
            get => _setting.GetItems();
        }

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

        private readonly SettingsHelper.ListedSetting _setting;

        public ListedSettingEditorViewModel(string settingName) 
        {
            _addCommand = new RelayCommand(AddItem);
            _removeCommand = new RelayCommand(RemoveItem);
            
            _settingName = settingName;
            _setting = new SettingsHelper.ListedSetting(settingName);
        }

        private void AddItem(object parameter)
        {
            if (string.IsNullOrEmpty(NewSettingValue))
            {
                return;
            }

            _setting.Add(NewSettingValue);
            OnPropertyChanged(nameof(Items));

            NewSettingValue = string.Empty;
        }

        private void RemoveItem(object parameter)
        {
            if (SelectedItem == null)
            {
                return;
            }

            _setting.Remove(SelectedItem);
            OnPropertyChanged(nameof(Items));
        }
    }
}
