using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Updaters;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private DiscordRpcController _discordRpcController;
        private SlotService _slotService;
        private LocalizationService<LocalizationFile> _localizationService;
        private SolutionHider _solutionHider;

        private GitRepositoryButtonSlot _gitRepositoryButtonSlot;

        public string Version => VisualStudioHelper.GetExtensionVersion();
        
        public bool UpdateNotificationsEnabled
        {
            get => bool.Parse(Settings.Default.UpdateNotifications);
            set => Settings.Default.UpdateNotifications = value.ToString();
        }

        public bool RichPresenceEnabled
        {
            get => _discordRpcController.Enabled;
            set
            {
                _discordRpcController.Enabled = value;
                Settings.Default.RichPresenceEnabled = value.ToString();
            }
        }

        public bool SecretSolution
        {
            get => _discordRpcController.Secret;
            set
            {
                _discordRpcController.Secret = value;
                _solutionHider.SetCurrentSolutionSecret(value);
                
                OnPropertyChanged(nameof(SecretSolution));
            }
        }

        public int UpdateTimeout
        {
            get => int.Parse(Settings.Default.UpdateTimeout);
            set
            {
                Settings.Default.UpdateTimeout = value.ToString();
                OnPropertyChanged(nameof(UpdateTimeout));
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

        public AssetSlot LargeIconSlot
        {
            get => (AssetSlot) _discordRpcController.GetSlotOfUpdater<LargeIconUpdater>();
            set
            {
                Settings.Default.LargeIconSlot = value.GetType().Name;
                _discordRpcController.SetSlot<LargeIconUpdater>(value);

                OnPropertyChanged(nameof(LargeIconSlot));
            }
        }

        public AssetSlot SmallIconSlot
        {
            get => (AssetSlot) _discordRpcController.GetSlotOfUpdater<SmallIconUpdater>();
            set
            {
                Settings.Default.SmallIconSlot = value.GetType().Name;
                _discordRpcController.SetSlot<SmallIconUpdater>(value);

                OnPropertyChanged(nameof(SmallIconSlot));
            }
        }

        public TextSlot StateSlot
        {
            get => (TextSlot)_discordRpcController.GetSlotOfUpdater<StateUpdater>();
            set
            {
                Settings.Default.StateSlot = value.GetType().Name;
                _discordRpcController.SetSlot<StateUpdater>(value);

                OnPropertyChanged(nameof(StateSlot));
            }
        }

        public TextSlot DetailsSlot
        {
            get => (TextSlot) _discordRpcController.GetSlotOfUpdater<DetailsUpdater>();
            set
            {
                Settings.Default.DetailsSlot = value.GetType().Name;
                _discordRpcController.SetSlot<DetailsUpdater>(value);

                OnPropertyChanged(nameof(DetailsSlot));
            }
        }


        public TimerSlot TimerSlot
        {
            get => (TimerSlot) _discordRpcController.GetSlotOfUpdater<TimerUpdater>();
            set
            {
                Settings.Default.TimerSlot = value.GetType().Name;
                _discordRpcController.SetSlot<TimerUpdater>(value);

                OnPropertyChanged(nameof(TimerSlot));
            }
        }

        public ButtonSlot FirstButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<FirstButtonUpdater>();
            set
            {
                Settings.Default.FirstButtonSlot = value.GetType().Name;
                _discordRpcController.SetSlot<FirstButtonUpdater>(value);

                OnPropertyChanged(nameof(FirstButtonSlot));
            }
        }

        public ButtonSlot SecondButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<SecondButtonUpdater>();
            set
            {
                Settings.Default.SecondButtonSlot = value.GetType().Name;
                _discordRpcController.SetSlot<SecondButtonUpdater>(value);

                OnPropertyChanged(nameof(SecondButtonSlot));
            }
        }

        public bool HasRepository => _gitRepositoryButtonSlot.HasRepository;

        public bool PrivateRepository
        {
            get => _gitRepositoryButtonSlot.IsPrivateRepository;
            set
            {
                _gitRepositoryButtonSlot.IsPrivateRepository = value;
                OnPropertyChanged(nameof(PrivateRepository));
            }
        }

        public IReadOnlyList<AssetSlot> AvailableAssetSlots { get; set; }
        public IReadOnlyList<TextSlot> AvailableTextSlots { get; set; }
        public IReadOnlyList<TimerSlot> AvailableTimerSlots { get; set; }
        public IReadOnlyList<ButtonSlot> AvailableButtonSlots { get; set; }

        public SettingsViewModel()
        {
            _discordRpcController = ServiceRepository.Default.GetService<DiscordRpcController>();
            _slotService = ServiceRepository.Default.GetService<SlotService>();
            _solutionHider = ServiceRepository.Default.GetService<SolutionHider>();
            
            _gitRepositoryButtonSlot = _slotService.GetSlotsOfType<GitRepositoryButtonSlot>().First();

            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            OnPropertyChanged(nameof(Localizations));

            AvailableAssetSlots = _slotService.GetSlotsOfType<AssetSlot>();
            OnPropertyChanged(nameof(AvailableAssetSlots));

            AvailableTextSlots = _slotService.GetSlotsOfType<TextSlot>();
            OnPropertyChanged(nameof(AvailableTextSlots));

            AvailableTimerSlots = _slotService.GetSlotsOfType<TimerSlot>();
            OnPropertyChanged(nameof(AvailableTimerSlots));

            AvailableButtonSlots = _slotService.GetSlotsOfType<ButtonSlot>();
            OnPropertyChanged(nameof(AvailableButtonSlots));
        }
    }
}
