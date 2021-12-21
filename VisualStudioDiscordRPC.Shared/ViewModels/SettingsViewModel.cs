using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

                IconEnum = new ObservableCollection<RichPresenceWrapper.Icon>(_iconEnum);
                TextEnum = new ObservableCollection<RichPresenceWrapper.Text>(_textEnum);
            } 
        }

        private ObservableCollection<RichPresenceWrapper.Icon> _iconEnum;
        public ObservableCollection<RichPresenceWrapper.Icon> IconEnum
        {
            get => _iconEnum;
            set => SetProperty(ref _iconEnum, value, nameof(IconEnum));
        }

        private ObservableCollection<RichPresenceWrapper.Text> _textEnum;
        public ObservableCollection<RichPresenceWrapper.Text> TextEnum
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

        public SettingsViewModel()
        {
            LocalizationManager = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            IconEnum = new ObservableCollection<RichPresenceWrapper.Icon>(
                Enum.GetValues(typeof(RichPresenceWrapper.Icon)) as IEnumerable<RichPresenceWrapper.Icon>);
            TextEnum = new ObservableCollection<RichPresenceWrapper.Text>(
                Enum.GetValues(typeof(RichPresenceWrapper.Text)) as IEnumerable<RichPresenceWrapper.Text>);
        }
    }
}
