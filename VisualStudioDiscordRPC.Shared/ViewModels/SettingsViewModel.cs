using System.Collections.Generic;
using System.Collections.ObjectModel;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private DiscordRpcController _discordRpcController;
        private SlotService _slotService;
        private LocalizationService<LocalizationFile> _localizationService;

        public bool RichPresenceEnabled
        {
            get => _discordRpcController.Enabled;
            set
            {
                _discordRpcController.Enabled = value;
                Settings.Default.RichPresenceEnabled = value.ToString();
            }
        }

        public string DiscordAppId
        {
            get => Settings.Default.ApplicationID;
            set
            {
                Settings.Default.ApplicationID = value;
                OnPropertyChanged(nameof(DiscordAppId));
            }
        }

        public IList<LocalizationFile> Localizations => _localizationService.Localizations;

        public LocalizationFile SelectedLocalization
        {
            get => _localizationService.Current;
            set
            {
                _localizationService.SelectLanguage(value.LanguageName);
                Settings.Default.Language = value.LanguageName;
                OnPropertyChanged(nameof(SelectedLocalization));
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

        public IReadOnlyList<AssetSlot> AvailableAssetSlots { get; set; }
        public IReadOnlyList<TextSlot> AvailableTextSlots { get; set; }

        public SettingsViewModel()
        {
            _discordRpcController = ServiceRepository.Default.GetService<DiscordRpcController>();
            _slotService = ServiceRepository.Default.GetService<SlotService>();

            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            OnPropertyChanged(nameof(Localizations));

            _largeIconSlot = _slotService.GetAssetSlotByName(Settings.Default.LargeIcon);
            _smallIconSlot = _slotService.GetAssetSlotByName(Settings.Default.SmallIcon);

            _stateSlot = _slotService.GetTextSlotByName(Settings.Default.TitleText);
            _detailsSlot = _slotService.GetTextSlotByName(Settings.Default.SubTitleText);

            AvailableAssetSlots = _slotService.AssetSlots;
            OnPropertyChanged(nameof(AvailableAssetSlots));

            AvailableTextSlots = _slotService.TextSlots;
            OnPropertyChanged(nameof(AvailableTextSlots));
        }
    }
}
