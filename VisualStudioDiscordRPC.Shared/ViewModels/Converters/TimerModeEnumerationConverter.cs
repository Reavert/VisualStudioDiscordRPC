using System;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Interfaces;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class TimerModeEnumerationConverter : IValueConverter
    {
        private readonly ILocalizationService<LocalizationFile> _localizationService;

        public TimerModeEnumerationConverter()
        {
            _localizationService =
                ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        private string GetTimerModeValue(RichPresenceWrapper.TimerMode iconValue)
        {
            switch (iconValue)
            {
                case RichPresenceWrapper.TimerMode.Disabled:
                    return _localizationService.Current.None;
                case RichPresenceWrapper.TimerMode.File:
                    return _localizationService.Current.File;
                case RichPresenceWrapper.TimerMode.Project:
                    return _localizationService.Current.Project;
                case RichPresenceWrapper.TimerMode.Solution:
                    return _localizationService.Current.SolutionName;
                default:
                    throw new ArgumentException($"No suitable string for value {iconValue}");
            }
        }

        private RichPresenceWrapper.TimerMode GetTimerModeValueBack(string timerModeString)
        {
            if (timerModeString == _localizationService.Current.None)
            {
                return RichPresenceWrapper.TimerMode.Disabled;
            }

            if (timerModeString == _localizationService.Current.File)
            {
                return RichPresenceWrapper.TimerMode.File;
            }

            if (timerModeString == _localizationService.Current.Project)
            {
                return RichPresenceWrapper.TimerMode.Project;
            }

            if (timerModeString == _localizationService.Current.SolutionName)
            {
                return RichPresenceWrapper.TimerMode.Solution;
            }

            throw new ArgumentException($"No suitable value for string {timerModeString}");
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textValue = value as RichPresenceWrapper.TimerMode? ?? RichPresenceWrapper.TimerMode.Disabled;

            return GetTimerModeValue(textValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textValue = value as string;

            return GetTimerModeValueBack(textValue);
        }
    }
}
