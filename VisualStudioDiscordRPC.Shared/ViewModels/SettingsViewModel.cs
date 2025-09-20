using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Localization.Interfaces;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services;
using VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs;
using VisualStudioDiscordRPC.Shared.Nests;
using VisualStudioDiscordRPC.Shared.Utils;
using System.Windows.Input;
using System.Windows.Forms;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly VsObserver _vsObserver;
        private readonly GitObserver _gitObserver;
        private readonly SettingsService _settingsService;
        private readonly DiscordRpcController _discordRpcController;
        private readonly PlugService _plugService;
        private readonly LocalizationService _localizationService;
        private readonly SolutionSecrecyService _solutionSecrecyService;
        private readonly RepositorySecrecyService _repositorySecrecyService;

        public string Version => VisualStudioHelper.GetExtensionVersion();
        
        public bool UpdateNotificationsEnabled
        {
            get => _settingsService.Read<bool>(SettingsKeys.UpdateNotifications);
            set => _settingsService.Set(SettingsKeys.UpdateNotifications, value);
        }

        public bool DetectIdlingEnabled
        {
            get => _settingsService.Read<bool>(SettingsKeys.DetectIdling);
            set
            {
                _settingsService.Set(SettingsKeys.DetectIdling, value);
                OnPropertyChanged(nameof(DetectIdlingEnabled));
            }
        }

        public bool ResetTimersAfterIdling
        {
            get => _settingsService.Read<bool>(SettingsKeys.ResetTimersAfterIdling);
            set
            {
                _settingsService.Set(SettingsKeys.ResetTimersAfterIdling, value);
                OnPropertyChanged(nameof(ResetTimersAfterIdling));
            }
        }

        public string IdleTime
        {
            get => _settingsService.Read(SettingsKeys.IdleTime, SettingsDefaults.DefaultIdleTime).ToString();
            set => _settingsService.Set(SettingsKeys.IdleTime, long.Parse(value));
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

        public IReadOnlyList<ILocalizationFile> Localizations => _localizationService.Localizations;

        public ILocalizationFile SelectedLocalization
        {
            get => _localizationService.Current;
            set
            {
                _localizationService.SelectLanguage(value.LanguageName);
                _settingsService.Set(SettingsKeys.Language, value.LanguageName);
                OnPropertyChanged(nameof(SelectedLocalization));
            }
        }

        public BaseAssetPlug LargeIconPlug
        {
            get => (BaseAssetPlug) _discordRpcController.GetPlugOfNest<LargeIconNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.LargeIconPlug, value.GetId());
                _discordRpcController.SetPlug<LargeIconNest>(value);

                OnPropertyChanged(nameof(LargeIconPlug));
            }
        }

        public BaseAssetPlug SmallIconPlug
        {
            get => (BaseAssetPlug) _discordRpcController.GetPlugOfNest<SmallIconNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.SmallIconPlug, value.GetId());
                _discordRpcController.SetPlug<SmallIconNest>(value);

                OnPropertyChanged(nameof(SmallIconPlug));
            }
        }

        public BaseTextPlug StatePlug
        {
            get => (BaseTextPlug)_discordRpcController.GetPlugOfNest<StateNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.StatePlug, value.GetId());
                _discordRpcController.SetPlug<StateNest>(value);

                OnPropertyChanged(nameof(StatePlug));
            }
        }

        public BaseTextPlug DetailsPlug
        {
            get => (BaseTextPlug) _discordRpcController.GetPlugOfNest<DetailsNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.DetailsPlug, value.GetId());
                _discordRpcController.SetPlug<DetailsNest>(value);

                OnPropertyChanged(nameof(DetailsPlug));
            }
        }

        public BaseTimerPlug TimerPlug
        {
            get => (BaseTimerPlug) _discordRpcController.GetPlugOfNest<TimerNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.TimerPlug, value.GetId());
                _discordRpcController.SetPlug<TimerNest>(value);

                OnPropertyChanged(nameof(TimerPlug));
            }
        }

        public BaseButtonPlug FirstButtonPlug
        {
            get => (BaseButtonPlug) _discordRpcController.GetPlugOfNest<FirstButtonNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.FirstButtonPlug, value.GetId());
                _discordRpcController.SetPlug<FirstButtonNest>(value);

                OnPropertyChanged(nameof(FirstButtonPlug));
            }
        }

        public BaseButtonPlug SecondButtonPlug
        {
            get => (BaseButtonPlug) _discordRpcController.GetPlugOfNest<SecondButtonNest>();
            set
            {
                if (value == null)
                    return;

                _settingsService.Set(SettingsKeys.SecondButtonPlug, value.GetId());
                _discordRpcController.SetPlug<SecondButtonNest>(value);

                OnPropertyChanged(nameof(SecondButtonPlug));
            }
        }

        public IReadOnlyList<BaseAssetPlug> AvailableAssetPlugs => _plugService.GetPlugsOfType<BaseAssetPlug>();
        public IReadOnlyList<BaseTextPlug> AvailableTextPlugs => _plugService.GetPlugsOfType<BaseTextPlug>();
        public IReadOnlyList<BaseTimerPlug> AvailableTimerPlugs => _plugService.GetPlugsOfType<BaseTimerPlug>();
        public IReadOnlyList<BaseButtonPlug> AvailableButtonPlugs => _plugService.GetPlugsOfType<BaseButtonPlug>();

        public RelayCommand ShowSecretSolutionsCommand { get; }
        public RelayCommand ShowPrivateRepositoriesCommand { get; }
        public RelayCommand ShowCustomTextPlugsEditorCommand { get; }
        public RelayCommand SelectSecretFolderCommand { get; }

        public SettingsViewModel()
        {
            _vsObserver = ServiceRepository.Default.GetService<VsObserver>();
            _gitObserver = ServiceRepository.Default.GetService<GitObserver>();
            _settingsService = ServiceRepository.Default.GetService<SettingsService>();
            _discordRpcController = ServiceRepository.Default.GetService<DiscordRpcController>();
            _plugService = ServiceRepository.Default.GetService<PlugService>();
            _solutionSecrecyService = ServiceRepository.Default.GetService<SolutionSecrecyService>();
            _repositorySecrecyService = ServiceRepository.Default.GetService<RepositorySecrecyService>();
            _localizationService = ServiceRepository.Default.GetService<LocalizationService>();

            OnPropertyChanged(nameof(Localizations));
            OnPropertyChanged(nameof(AvailableAssetPlugs));
            OnPropertyChanged(nameof(AvailableTextPlugs));
            OnPropertyChanged(nameof(AvailableTimerPlugs));
            OnPropertyChanged(nameof(AvailableButtonPlugs));

            ShowSecretSolutionsCommand = new RelayCommand(ShowSecretSolutionsEditor);
            ShowPrivateRepositoriesCommand = new RelayCommand(ShowPrivateRepositoriesEditor);
            ShowCustomTextPlugsEditorCommand = new RelayCommand(ShowCustomTextPlugsEditor);
            SelectSecretFolderCommand = new RelayCommand(SelectSecretFolder);
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

        private void ShowCustomTextPlugsEditor(object parameter)
        {
            var viewModel = new CustomTextPlugsEditorViewModel();
            var view = new CustomTextPlugsEditor(viewModel);

            view.ShowDialog();

            _plugService.SaveCustomTextPlugsData();
            _discordRpcController.RefreshAll();

            OnPropertyChanged(nameof(AvailableTextPlugs));
        }

        private void SelectSecretFolder(object parameter)
        {
            using (var folderDialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                folderDialog.Description = "Select folder to hide all solutions in it";
                folderDialog.ShowNewFolderButton = false;
                
                if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        _solutionSecrecyService.AddSecretSolution(selectedPath);
                        OnPropertyChanged(nameof(SecretSolution));
                    }
                }
            }
        }
    }
}
