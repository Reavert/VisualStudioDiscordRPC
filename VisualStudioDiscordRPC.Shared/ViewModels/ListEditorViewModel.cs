using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class ListEditorViewModel : ViewModelBase
    {
        private readonly RelayCommand _addCommand;
        private readonly RelayCommand _removeCommand;

        private string _newSettingValue;
        private IStringCollectionProvider _stringCollectionProvider;
        private readonly ObservableCollection<string> _items;

        public ObservableCollection<string> Items => _items;

        public string SelectedItem { get; set; }
        
        public string NewSettingValue 
        {
            get => _newSettingValue;
            set => SetProperty(ref _newSettingValue, value);
        }

        public RelayCommand AddCommand => _addCommand;

        public RelayCommand RemoveCommand => _removeCommand;

        public ListEditorViewModel(IStringCollectionProvider stringCollectionProvider)
        {
            _addCommand = new RelayCommand(AddItem);
            _removeCommand = new RelayCommand(RemoveItem);
            
            _stringCollectionProvider = stringCollectionProvider;
            _items = new ObservableCollection<string>(stringCollectionProvider.Items);
        }

        private void AddItem(object parameter)
        {
            if (string.IsNullOrEmpty(NewSettingValue))
            {
                return;
            }

            _stringCollectionProvider.Add(NewSettingValue);
            _items.Add(NewSettingValue);
            
            NewSettingValue = string.Empty;
        }

        private void RemoveItem(object parameter)
        {
            if (SelectedItem == null)
            {
                return;
            }

            _stringCollectionProvider.Remove(SelectedItem);
            _items.Remove(SelectedItem);
        }
    }
}
