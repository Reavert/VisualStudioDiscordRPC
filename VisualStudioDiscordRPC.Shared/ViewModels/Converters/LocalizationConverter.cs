using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class LocalizationConverter : IValueConverter
    {
        private readonly LocalizationService<LocalizationFile> _localizationService;
        private Dictionary<string, Type> _typesCache;

        public LocalizationConverter()
        {
            _localizationService =
                ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            _typesCache = new Dictionary<string, Type>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Type type = value?.GetType();
            string translation = _localizationService.Current.GetTypeValue(type, value);

            if (!_typesCache.ContainsKey(translation))
            {
                _typesCache.Add(translation, type);
            }
            
            return translation;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _typesCache[(string) value];
        }
    }
}
