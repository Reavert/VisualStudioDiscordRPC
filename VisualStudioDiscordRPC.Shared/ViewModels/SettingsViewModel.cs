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
                WorkTimerModeEnum = new ObservableCollection<RichPresenceWrapper.TimerMode>(_workTimerModeEnum);
            } 
        }

        public bool RichPresenceEnabled
        {
            get => _wrapper.Enabled;
            set
            {
                _wrapper.Enabled = value;
                _wrapper.Update();
                Settings.Default.RichPresenceEnabled = value.ToString();
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

        private ObservableCollection<RichPresenceWrapper.TimerMode> _workTimerModeEnum;

        public ObservableCollection<RichPresenceWrapper.TimerMode> WorkTimerModeEnum
        {
            get => _workTimerModeEnum;
            set => SetProperty(ref _workTimerModeEnum, value, nameof(WorkTimerModeEnum));
        }

        public RichPresenceWrapper.Icon SelectedLargeIcon
        {
            get => _wrapper.LargeIcon;
            set
            {
                _wrapper.LargeIcon = value;
                _wrapper.Update();
                Settings.Default.LargeIcon = SettingsHelper.Instance.IconEnumMap.GetString(value);
            }
        }

        public RichPresenceWrapper.Icon SelectedSmallIcon
        {
            get => _wrapper.SmallIcon;
            set
            {
                _wrapper.SmallIcon = value;
                _wrapper.Update();
                Settings.Default.SmallIcon = SettingsHelper.Instance.IconEnumMap.GetString(value);
            } 
        }

        public RichPresenceWrapper.Text SelectedTitleText
        {
            get => _wrapper.TitleText;
            set
            {
                _wrapper.TitleText = value;
                _wrapper.Update();
                Settings.Default.TitleText = SettingsHelper.Instance.TextEnumMap.GetString(value);
            }
        }

        public RichPresenceWrapper.Text SelectedSubTitleText
        {
            get => _wrapper.SubTitleText;
            set
            {
                _wrapper.SubTitleText = value;
                _wrapper.Update();
                Settings.Default.SubTitleText = SettingsHelper.Instance.TextEnumMap.GetString(value);
            } 
        }

        public bool GitLinkVisible
        {
            get => _wrapper.GitLinkVisible;
            set
            {
                _wrapper.GitLinkVisible = value;
                _wrapper.Update();
                Settings.Default.GitLinkVisible = value.ToString();
            }
        }

        public RichPresenceWrapper.TimerMode SelectedWorkTimerMode
        {
            get => _wrapper.WorkTimerMode;
            set
            {
                _wrapper.WorkTimerMode = value;
                _wrapper.Update();
                Settings.Default.WorkTimerMode = SettingsHelper.Instance.TimerModeEnumMap.GetString(value);
            }
        }

        private ObservableCollection<T> ToObservableCollection<T>() where T : Enum
        {
            return new ObservableCollection<T>(
                Enum.GetValues(typeof(T)) as IEnumerable<T>);
        }

        public SettingsViewModel()
        {
            LocalizationManager = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();

            IconEnum = ToObservableCollection<RichPresenceWrapper.Icon>();
            TextEnum = ToObservableCollection<RichPresenceWrapper.Text>();
            WorkTimerModeEnum = ToObservableCollection<RichPresenceWrapper.TimerMode>();
        }
    }
}
