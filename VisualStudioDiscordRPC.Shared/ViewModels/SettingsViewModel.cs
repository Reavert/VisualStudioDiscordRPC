using System;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Interfaces;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private RichPresenceWrapper _wrapper;
        public RichPresenceWrapper Wrapper
        {
            get => _wrapper;
            set => SetProperty(ref _wrapper, value, nameof(Wrapper));
        }

        private ILocalizationService<LocalizationFile> _localizationManager;

        public ILocalizationService<LocalizationFile> LocalizationManager
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

        public RichPresenceWrapper.Icon SelectedLargeIcon
        {
            get => _wrapper.LargeIcon;
            set => _wrapper.LargeIcon = value;
        }

        public RichPresenceWrapper.Icon SelectedSmallIcon
        {
            get => _wrapper.SmallIcon;
            set => _wrapper.SmallIcon = value;
        }

        public RichPresenceWrapper.Text SelectedTitleText
        {
            get => _wrapper.TitleText;
            set => _wrapper.TitleText = value;
        }

        public RichPresenceWrapper.Text SelectedSubTitleText
        {
            get => _wrapper.SubTitleText;
            set => _wrapper.SubTitleText = value;
        }

        private string GetIconValue(RichPresenceWrapper.Icon iconValue)
        {
            switch (iconValue)
            {
                case RichPresenceWrapper.Icon.None:
                    return LocalizationManager.Current.None;
                case RichPresenceWrapper.Icon.FileExtension:
                    return LocalizationManager.Current.FileExtension;
                case RichPresenceWrapper.Icon.VisualStudioVersion:
                    return LocalizationManager.Current.VisualStudioVersion;
                default:
                    return LocalizationManager.Current.None;
            }
        }

        public SettingsViewModel()
        {
            LocalizationManager = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            IconEnum = Enum.GetValues(typeof(RichPresenceWrapper.Icon)) as IEnumerable<RichPresenceWrapper.Icon>;
            TextEnum = Enum.GetValues(typeof(RichPresenceWrapper.Text)) as IEnumerable<RichPresenceWrapper.Text>;

        }
    }
}
