using System.Collections.ObjectModel;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class ListEditorViewModel : ViewModelBase
    {
        public RelayCommand AddCommand => _addCommand;

        public RelayCommand RemoveCommand => _removeCommand;

        public ObservableCollection<string> Items => _items;

        public string SelectedItem { get; set; }

        public string NewItemValue
        {
            get => _newItemValue;
            set => SetProperty(ref _newItemValue, value);
        }

        private readonly RelayCommand _addCommand;
        private readonly RelayCommand _removeCommand;

        private readonly ObservableCollection<string> _items;
        private readonly IStringCollectionProvider _stringCollectionProvider;

        private string _newItemValue;
        
        public ListEditorViewModel(IStringCollectionProvider stringCollectionProvider)
        {
            _addCommand = new RelayCommand(AddItem);
            _removeCommand = new RelayCommand(RemoveItem);
            
            _stringCollectionProvider = stringCollectionProvider;
            _items = new ObservableCollection<string>(stringCollectionProvider.Items);
        }

        private void AddItem(object parameter)
        {
            if (string.IsNullOrEmpty(NewItemValue))
            {
                return;
            }

            _stringCollectionProvider.Add(NewItemValue);
            _items.Add(NewItemValue);
            
            NewItemValue = string.Empty;
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
