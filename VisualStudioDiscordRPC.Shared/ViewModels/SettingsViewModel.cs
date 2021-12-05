using VisualStudioDiscordRPC.Shared.Localization.Interfaces;
using VisualStudioDiscordRPC.Shared.Localization.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ILocalizationManager<LocalizationFile> _localizationManager;

        public ILocalizationManager<LocalizationFile> LocalizationManager
        {
            get => _localizationManager;
            set => SetProperty(ref _localizationManager, value, nameof(LocalizationManager));
        }

        public LocalizationFile SelectedLocalization
        {
            get => _localizationManager?.Current;
            set
            {
                _localizationManager?.SelectLanguage(value.LanguageName);
                Settings.Default.Language = value.LanguageName;
            } 
        }

        public SettingsViewModel()
        { }
    }
}
