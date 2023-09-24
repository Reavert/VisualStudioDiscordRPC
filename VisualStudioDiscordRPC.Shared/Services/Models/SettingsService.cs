using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class SettingsService
    {
        private const string SettingsFileName = "settings.json";

        private readonly string ApplicationDataPath;
        private readonly string SettingsPath;

        private readonly Dictionary<string, object> _settingsMap = new Dictionary<string, object>();

        public SettingsService() 
        {
            ApplicationDataPath = PathHelper.GetApplicationDataPath();
            SettingsPath = Path.Combine(ApplicationDataPath, SettingsFileName);

            if (!Directory.Exists(ApplicationDataPath))
            {
                Directory.CreateDirectory(ApplicationDataPath);
            }

            if (!File.Exists(SettingsPath))
            {
                PopulateDefaultProperties();
                Save();
            }
            else
            {
                string json = File.ReadAllText(SettingsPath);
                _settingsMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }

        }

        public T Read<T>(string key, T defaultValue = default)
        {
            if (!_settingsMap.TryGetValue(key, out var value))
                return defaultValue;
            
            return (T) value;
        }

        public void Set<T>(string key, T value)
        {
            _settingsMap[key] = value;
        }

        public void Save()
        {
            var settingsJson = JsonConvert.SerializeObject(_settingsMap, Formatting.Indented);
            File.WriteAllText(SettingsPath, settingsJson);
        }

        private void PopulateDefaultProperties()
        {
            _settingsMap[SettingsKeys.RichPresenceEnabled] = true;
            _settingsMap[SettingsKeys.Language] = "English";
            _settingsMap[SettingsKeys.LargeIconSlot] = typeof(ExtensionIconSlot).Name;
            _settingsMap[SettingsKeys.SmallIconSlot] = typeof(VisualStudioVersionIconSlot).Name;
            _settingsMap[SettingsKeys.DetailsSlot] = typeof(FileNameTextSlot).Name;
            _settingsMap[SettingsKeys.StateSlot] = typeof(SolutionNameTextSlot).Name;
            _settingsMap[SettingsKeys.TimerSlot] = typeof(WithinFilesTimerSlot).Name;
            _settingsMap[SettingsKeys.FirstButtonSlot] = typeof(GitRepositoryButtonSlot).Name;
            _settingsMap[SettingsKeys.SecondButtonSlot] = typeof(NoneButtonSlot).Name;
            _settingsMap[SettingsKeys.ApplicationID] = DiscordRpcController.DefaultApplicationId;
            _settingsMap[SettingsKeys.UpdateTimeout] = (long) 1000;
            _settingsMap[SettingsKeys.Version] = "1.0.0";
            _settingsMap[SettingsKeys.UpdateNotifications] = true;
            _settingsMap[SettingsKeys.TranslationsPath] = "Translations/";
        }
    }
}
