using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private DiscordRpcController _discordRpcController;
        private SlotService _slotService;

        public string DiscordAppId
        {
            get => Settings.Default.ApplicationID;
            set
            {
                Settings.Default.ApplicationID = value;
                OnPropertyChanged(nameof(DiscordAppId));
            }
        }

        private AssetSlot _largeIconSlot;
        public AssetSlot LargeIconSlot
        {
            get => _largeIconSlot;
            set
            {
                SetProperty(ref _largeIconSlot, value);
                Settings.Default.LargeIcon = value.GetType().Name;

                _discordRpcController.LargeIconSlot = value;
            }
        }

        private AssetSlot _smallIconSlot;
        public AssetSlot SmallIconSlot
        {
            get => _smallIconSlot;
            set
            {
                SetProperty(ref _smallIconSlot, value);
                Settings.Default.SmallIcon = value.GetType().Name;

                _discordRpcController.SmallIconSlot = value;
            }
        }

        private TextSlot _stateSlot;
        public TextSlot StateSlot
        {
            get => _stateSlot;
            set
            {
                SetProperty(ref _stateSlot, value);
                Settings.Default.TitleText= value.GetType().Name;

                _discordRpcController.StateSlot = value;
            }
        }

        private TextSlot _detailsSlot;
        public TextSlot DetailsSlot
        {
            get => _detailsSlot;
            set
            {
                SetProperty(ref _detailsSlot, value);
                Settings.Default.SubTitleText = value.GetType().Name;

                _discordRpcController.DetailsSlot = value;
            }
        }

        public ObservableCollection<AssetSlot> AvailableAssetSlots { get; set; }
        public ObservableCollection<TextSlot> AvailableTextSlots { get; set; }

        public SettingsViewModel()
        {
            _discordRpcController = ServiceRepository.Default.GetService<DiscordRpcController>();
            _slotService = ServiceRepository.Default.GetService<SlotService>();

            _largeIconSlot = _slotService.GetAssetSlotByName(Settings.Default.LargeIcon);
            _smallIconSlot = _slotService.GetAssetSlotByName(Settings.Default.SmallIcon);

            _stateSlot = _slotService.GetTextSlotByName(Settings.Default.TitleText);
            _detailsSlot = _slotService.GetTextSlotByName(Settings.Default.SubTitleText);

            AvailableAssetSlots = new ObservableCollection<AssetSlot>();
            foreach (AssetSlot assetSlot in _slotService.AssetSlots)
            {
                AvailableAssetSlots.Add(assetSlot);
            }

            AvailableTextSlots = new ObservableCollection<TextSlot>();
            foreach (TextSlot textSlot in _slotService.TextSlots)
            {
                AvailableTextSlots.Add(textSlot);
            }
        }
    }
}
