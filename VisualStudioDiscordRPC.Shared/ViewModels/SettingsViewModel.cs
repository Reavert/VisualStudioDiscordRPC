using System;
using System.Collections.Generic;
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

        private IEnumerable<RichPresenceWrapper.Icon> _iconEnum;
        public IEnumerable<RichPresenceWrapper.Icon> IconEnum
        {
            get => _iconEnum;
            set => SetProperty(ref _iconEnum, value, nameof(IconEnum));
        }

        private IEnumerable<RichPresenceWrapper.Text> _textEnum;
        public IEnumerable<RichPresenceWrapper.Text> TextEnum
        {
            get => _textEnum;
            set => SetProperty(ref _textEnum, value, nameof(TextEnum));
        }

        public RichPresenceWrapper.Icon SelectedLargeIcon { get; set; }
        public RichPresenceWrapper.Icon SelectedSmallIcon { get; set; }

        public RichPresenceWrapper.Text SelectedTitleText { get; set; }
        public RichPresenceWrapper.Text SelectedSubTitleText { get; set; }

        public SettingsViewModel()
        { 
            IconEnum = Enum.GetValues(typeof(RichPresenceWrapper.Icon)) as IEnumerable<RichPresenceWrapper.Icon>;
            TextEnum = Enum.GetValues(typeof(RichPresenceWrapper.Text)) as IEnumerable<RichPresenceWrapper.Text>;
        }
    }
}
