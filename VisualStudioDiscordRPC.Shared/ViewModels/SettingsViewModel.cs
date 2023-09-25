using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
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
        private readonly VsObserver _vsObserver;
        private readonly GitObserver _gitObserver;
        private readonly SettingsService _settingsService;
        private readonly DiscordRpcController _discordRpcController;
        private readonly SlotService _slotService;
        private readonly LocalizationService<LocalizationFile> _localizationService;
        private readonly SolutionSecrecyService _solutionSecrecyService;
        private readonly RepositorySecrecyService _repositorySecrecyService;

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
            get
            {
                Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

                if (_vsObserver.DTE.Solution == null)
                    return false;

                return _solutionSecrecyService.IsSolutionSecret(_vsObserver.DTE.Solution.FullName);
            }
            set
            {
                Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

                if (_vsObserver.DTE.Solution == null)
                    return;

                string solutionFullName = _vsObserver.DTE.Solution.FullName;

                if (value)
                    _solutionSecrecyService.AddSecretSolution(solutionFullName);
                else
                    _solutionSecrecyService.RemoveSecretSolution(solutionFullName);

                OnPropertyChanged(nameof(SecretSolution));
            }
        }

        public bool HasRepository => !string.IsNullOrEmpty(_gitObserver.RemoteUrl);

        public bool PrivateRepository
        {
            get => _repositorySecrecyService.IsRepositorySecret(_gitObserver.RemoteUrl);
            set
            {
                if (string.IsNullOrEmpty(_gitObserver.RemoteUrl))
                {
                    return;
                }

                string gitRemoteUrl = _gitObserver.RemoteUrl;

                if (value)
                    _repositorySecrecyService.AddSecretRepository(gitRemoteUrl);
                else
                    _repositorySecrecyService.RemoveSecretRepository(gitRemoteUrl);

                OnPropertyChanged(nameof(PrivateRepository));
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
                _settingsService.Set(SettingsKeys.LargeIconSlot, value.GetId());
                _discordRpcController.SetSlot<LargeIconUpdater>(value);

                OnPropertyChanged(nameof(LargeIconSlot));
            }
        }

        public AssetSlot SmallIconSlot
        {
            get => (AssetSlot) _discordRpcController.GetSlotOfUpdater<SmallIconUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.SmallIconSlot, value.GetId());
                _discordRpcController.SetSlot<SmallIconUpdater>(value);

                OnPropertyChanged(nameof(SmallIconSlot));
            }
        }

        public TextSlot StateSlot
        {
            get => (TextSlot)_discordRpcController.GetSlotOfUpdater<StateUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.StateSlot, value.GetId());
                _discordRpcController.SetSlot<StateUpdater>(value);

                OnPropertyChanged(nameof(StateSlot));
            }
        }

        public TextSlot DetailsSlot
        {
            get => (TextSlot) _discordRpcController.GetSlotOfUpdater<DetailsUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.DetailsSlot, value.GetId());
                _discordRpcController.SetSlot<DetailsUpdater>(value);

                OnPropertyChanged(nameof(DetailsSlot));
            }
        }

        public TimerSlot TimerSlot
        {
            get => (TimerSlot) _discordRpcController.GetSlotOfUpdater<TimerUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.TimerSlot, value.GetId());
                _discordRpcController.SetSlot<TimerUpdater>(value);

                OnPropertyChanged(nameof(TimerSlot));
            }
        }

        public ButtonSlot FirstButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<FirstButtonUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.FirstButtonSlot, value.GetId());
                _discordRpcController.SetSlot<FirstButtonUpdater>(value);

                OnPropertyChanged(nameof(FirstButtonSlot));
            }
        }

        public ButtonSlot SecondButtonSlot
        {
            get => (ButtonSlot) _discordRpcController.GetSlotOfUpdater<SecondButtonUpdater>();
            set
            {
                _settingsService.Set(SettingsKeys.SecondButtonSlot, value.GetId());
                _discordRpcController.SetSlot<SecondButtonUpdater>(value);

                OnPropertyChanged(nameof(SecondButtonSlot));
            }
        }

        public IReadOnlyList<AssetSlot> AvailableAssetSlots => _slotService.GetSlotsOfType<AssetSlot>();
        public IReadOnlyList<TextSlot> AvailableTextSlots => _slotService.GetSlotsOfType<TextSlot>();
        public IReadOnlyList<TimerSlot> AvailableTimerSlots => _slotService.GetSlotsOfType<TimerSlot>();
        public IReadOnlyList<ButtonSlot> AvailableButtonSlots => _slotService.GetSlotsOfType<ButtonSlot>();

        public RelayCommand ShowSecretSolutionsCommand { get; }
        public RelayCommand ShowPrivateRepositoriesCommand { get; }
        public RelayCommand ShowCustomTextSlotsEditorCommand { get; }

        public SettingsViewModel()
        {
            _vsObserver = ServiceRepository.Default.GetService<VsObserver>();
            _gitObserver = ServiceRepository.Default.GetService<GitObserver>();
            _settingsService = ServiceRepository.Default.GetService<SettingsService>();
            _discordRpcController = ServiceRepository.Default.GetService<DiscordRpcController>();
            _slotService = ServiceRepository.Default.GetService<SlotService>();
            _solutionSecrecyService = ServiceRepository.Default.GetService<SolutionSecrecyService>();
            _repositorySecrecyService = ServiceRepository.Default.GetService<RepositorySecrecyService>();
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            OnPropertyChanged(nameof(Localizations));
            OnPropertyChanged(nameof(AvailableAssetSlots));
            OnPropertyChanged(nameof(AvailableTextSlots));
            OnPropertyChanged(nameof(AvailableTimerSlots));
            OnPropertyChanged(nameof(AvailableButtonSlots));

            ShowSecretSolutionsCommand = new RelayCommand(ShowSecretSolutionsEditor);
            ShowPrivateRepositoriesCommand = new RelayCommand(ShowPrivateRepositoriesEditor);
            ShowCustomTextSlotsEditorCommand = new RelayCommand(ShowCustomTextSlotsEditor);
        }

        private void ShowSecretSolutionsEditor(object parameter)
        {
            var secretSolutionCollectionProvider = new SecretSolutionsCollectionProvider(_solutionSecrecyService);
            var listEditorViewModel = new ListEditorViewModel(secretSolutionCollectionProvider);
            var listEditorView = new ListEditorWindow(listEditorViewModel);

            listEditorView.ShowDialog();
            OnPropertyChanged(nameof(SecretSolution));
        }

        private void ShowPrivateRepositoriesEditor(object parameter)
        {
            var secretRepositoriesCollectionProvider = new SecretRepositoriesCollectonProvider(_repositorySecrecyService);
            var listEditorViewModel = new ListEditorViewModel(secretRepositoriesCollectionProvider);
            var listEditorView = new ListEditorWindow(listEditorViewModel);

            listEditorView.ShowDialog();
            OnPropertyChanged(nameof(PrivateRepository));
        }

        private void ShowCustomTextSlotsEditor(object paramter)
        {
            IEnumerable<CustomTextSlotData> customSlotsData = _slotService.GetCustomTextSlotsData();

            var viewModel = new CustomTextSlotsEditorViewModel(customSlotsData);
            var view = new CustomTextSlotsEditor(viewModel);

            view.ShowDialog();
            _slotService.SaveCustomTextSlotsData(viewModel.CustomTextSlots);
        }
    }
}
