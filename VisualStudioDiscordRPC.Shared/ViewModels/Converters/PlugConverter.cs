using System;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class PlugConverter : IValueConverter
    {
        private readonly LocalizationService _localizationService;

        public PlugConverter()
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CustomTextPlug customTextPlug)
                return customTextPlug.Name;

            string objectTypeName = value.GetType().Name;
            if (_localizationService.Current.LocalizedValues.TryGetValue(objectTypeName, out var localizedName))
            {
                return localizedName;
            }

            return objectTypeName;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
