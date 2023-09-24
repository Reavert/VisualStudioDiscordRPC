using System;
using System.Globalization;
using System.Windows.Data;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;

namespace VisualStudioDiscordRPC.Shared.ViewModels.Converters
{
    public class SlotConverter : IValueConverter
    {
        private readonly LocalizationService<LocalizationFile> _localizationService;

        public SlotConverter()
        {
            _localizationService =
                ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CustomTextSlot customTextSlot)
                return customTextSlot.Name;

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
