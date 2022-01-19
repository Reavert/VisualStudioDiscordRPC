using System;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Interfaces;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class IconEnumerationConverter : IValueConverter
    {
        private readonly ILocalizationService<LocalizationFile> _localizationService;

        public IconEnumerationConverter()
        {
            _localizationService = 
                ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        private string GetIconValue(RichPresenceWrapper.Icon iconValue)
        {
            switch (iconValue)
            {
                case RichPresenceWrapper.Icon.None:
                    return _localizationService.Current.None;
                case RichPresenceWrapper.Icon.FileExtension:
                    return _localizationService.Current.FileExtension;
                case RichPresenceWrapper.Icon.VisualStudioVersion:
                    return _localizationService.Current.VisualStudioVersion;
                default:
                    throw new ArgumentException($"No suitable string for value {iconValue}");
            }
        }

        private RichPresenceWrapper.Icon GetIconValueBack(string iconString)
        {
            if (iconString == _localizationService.Current.None)
            {
                return RichPresenceWrapper.Icon.None;
            }

            if (iconString == _localizationService.Current.VisualStudioVersion)
            {
                return RichPresenceWrapper.Icon.VisualStudioVersion;
            }

            if (iconString == _localizationService.Current.FileExtension)
            {
                return RichPresenceWrapper.Icon.FileExtension;
            }

            throw new ArgumentException($"No suitable value for string {iconString}");
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconValue = value as RichPresenceWrapper.Icon? ?? RichPresenceWrapper.Icon.None;

            return GetIconValue(iconValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconString = value as string;

            return GetIconValueBack(iconString);
        }
    }
}
