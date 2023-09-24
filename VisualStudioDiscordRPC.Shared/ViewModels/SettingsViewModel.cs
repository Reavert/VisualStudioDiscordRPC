using System;
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
        private SettingsService _settingsService;
        private DiscordRpcController _discordRpcController;
        private SlotService _slotService;
        private LocalizationService<LocalizationFile> _localizationService;
        private SolutionHider _solutionHider;

        private GitRepositoryButtonSlot _gitRepositoryButtonSlot;

        public string Version => VisualStudioHelper.GetExtensionVersion();
        
        public bool UpdateNotificationsEnabled
        {
            get => _settingsService.Read<bool>(SettingsKeys.UpdateNotifications);
            set => _settingsService.Set(SettingsKeys.UpdateNotifications, value);
        }

        public bool RichPresenceEnabled
        {
            get => _discordRpcController.Enabled;
            set
            {
                _discordRpcController.Enabled = value;
                _settingsService.Set(SettingsKeys.RichPresenceEnabled, value);
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
            get => Convert.ToInt32(_settingsService.Read<long>(SettingsKeys.UpdateTimeout));
            set
            {
                _settingsService.Set(SettingsKeys.UpdateTimeout, (long) value);
                OnPropertyChanged(nameof(UpdateTimeout));
            }
        }


        public string DiscordAppId
        {
            get => _settingsService.Read<string>(SettingsKeys.ApplicationID);
            set
            {
                _settingsService.Set(SettingsKeys.ApplicationID, value);
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
                _settingsService.Set(SettingsKeys.Language, value.LanguageName);
                OnPropertyChanged(nameof(SelectedLocalization));
            }
        }

        public AssetSlot LargeIconSlot
        {
            get => (AssetSlot) _discordRpcController.GetSlotOfUpdater<LargeIconUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.LargeIconSlot, value.GetType().Name);
                _discordRpcController.SetSlot<LargeIconUpdater>(value);

                OnPropertyChanged(nameof(LargeIconSlot));
            }
        }

        public AssetSlot SmallIconSlot
        {
            get => (AssetSlot) _discordRpcController.GetSlotOfUpdater<SmallIconUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.SmallIconSlot, value.GetType().Name);
                _discordRpcController.SetSlot<SmallIconUpdater>(value);

                OnPropertyChanged(nameof(SmallIconSlot));
            }
        }

        public TextSlot StateSlot
        {
            get => (TextSlot)_discordRpcController.GetSlotOfUpdater<StateUpdater>();
            set
            {
                //_settingsService.Set(SettingsKeys.StateSlot, value.GetType().Name);
                _discordRpcController.SetSlot<StateUpdater>(value);

                OnPropertyChanged(nameof(StateSlot));
            }
        }

        public TextSlot DetailsSlot
        {
            get => (TextSlot) _discordRpcController.GetSlotOfUpdater<DetailsUpdater>();
            set
            {
                //_settingsService.Set(SettingsKeys.DetailsSlot, value.GetType().Name);
                _discordRpcController.SetSlot<DetailsUpdater>(value);

                OnPropertyChanged(nameof(DetailsSlot));
            }
        }

        public TimerSlot TimerSlot
        {
            get => (TimerSlot) _discordRpcController.GetSlotOfUpdater<TimerUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.TimerSlot, value.GetType().Name);
                _discordRpcController.SetSlot<TimerUpdater>(value);

                OnPropertyChanged(nameof(TimerSlot));
            }
        }

        public ButtonSlot FirstButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<FirstButtonUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.FirstButtonSlot, value.GetType().Name);
                _discordRpcController.SetSlot<FirstButtonUpdater>(value);

                OnPropertyChanged(nameof(FirstButtonSlot));
            }
        }

        public ButtonSlot SecondButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<SecondButtonUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.SecondButtonSlot, value.GetType().Name);
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

        private readonly RelayCommand _showListSettingEditorCommand;
        public RelayCommand ShowListSettingEditorCommand => _showListSettingEditorCommand;

        private readonly RelayCommand _showCustomSlotsEditorCommand;
        public RelayCommand ShowCustomSlotsEditorCommand => _showCustomSlotsEditorCommand;

        public string[] PrivateRepositoriesEditingContext => _privateRepositoriesEditingContext;
        private readonly string[] _privateRepositoriesEditingContext =
        {
            nameof(SettingsKeys.PrivateRepositories),
            nameof(PrivateRepository)
        };

        public string[] SecretSolutionsEditingContext => _secretSolutionsEditingContext;
        private readonly string[] _secretSolutionsEditingContext =
        {
            nameof(SettingsKeys.SecretSolutions),
            nameof(SecretSolution)
        };

        
        public SettingsViewModel()
        {
            _settingsService = ServiceRepository.Default.GetService<SettingsService>();
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

            _showListSettingEditorCommand = new RelayCommand(ShowListSettingEditor);
        }

        private void ShowListSettingEditor(object parameter)
        {
            var stringParameters = (string[])parameter;

            string settingName = stringParameters[0];
            string propertyName = stringParameters[1];

            var listSettingViewModel = new ListedSettingEditorViewModel(settingName);
            var listSettingEditorWindow = new ListedSettingEditorWindow(listSettingViewModel);

            listSettingEditorWindow.ShowDialog();
            OnPropertyChanged(propertyName);
        }

        private static List<object> ToObjectList<T>(List<T> list)
        {
            return list.Select(item => (object)item).ToList();
        }

        private static List<T> ToConcreteList<T>(List<object> list)
        {
            return list.Select(item => (T)item).ToList();
        }
    }
}
