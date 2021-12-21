using System;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class TextEnumerationConverter : IValueConverter
    {
        private readonly LocalizationService<LocalizationFile> _localizationService;

        public TextEnumerationConverter()
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        private string GetTextValue(RichPresenceWrapper.Text textValue)
        {
            switch (textValue)
            {
                case RichPresenceWrapper.Text.None:
                    return _localizationService.Current.None;
                case RichPresenceWrapper.Text.FileName:
                    return _localizationService.Current.FileName;
                case RichPresenceWrapper.Text.ProjectName:
                    return _localizationService.Current.ProjectName;
                case RichPresenceWrapper.Text.FileExtension:
                    return _localizationService.Current.FileExtension;
                case RichPresenceWrapper.Text.VisualStudioVersion:
                    return _localizationService.Current.VisualStudioVersion;
                default:
                    return _localizationService.Current.None;
            }
        }

        private RichPresenceWrapper.Text GetTextValueBack(string textValue)
        {
            if (textValue == _localizationService.Current.None)
            {
                return RichPresenceWrapper.Text.None;
            }

            if (textValue == _localizationService.Current.FileName)
            {
                return RichPresenceWrapper.Text.FileName;
            }

            if (textValue == _localizationService.Current.ProjectName)
            {
                return RichPresenceWrapper.Text.ProjectName;
            }

            if (textValue == _localizationService.Current.FileExtension)
            {
                return RichPresenceWrapper.Text.FileExtension;
            }

            if (textValue == _localizationService.Current.VisualStudioVersion)
            {
                return RichPresenceWrapper.Text.VisualStudioVersion;
            }

            return RichPresenceWrapper.Text.None;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textValue = value as RichPresenceWrapper.Text? ?? RichPresenceWrapper.Text.None;

            return GetTextValue(textValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textValue = value as string;

            return GetTextValueBack(textValue);
        }
    }
}
