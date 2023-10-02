using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs;
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
            _settingsMap[SettingsKeys.LargeIconPlug] = typeof(ExtensionIconPlug).Name;
            _settingsMap[SettingsKeys.SmallIconPlug] = typeof(VisualStudioVersionIconPlug).Name;
            _settingsMap[SettingsKeys.DetailsPlug] = typeof(FileNameTextPlug).Name;
            _settingsMap[SettingsKeys.StatePlug] = typeof(SolutionNameTextPlug).Name;
            _settingsMap[SettingsKeys.TimerPlug] = typeof(FileScopeTimerPlug).Name;
            _settingsMap[SettingsKeys.FirstButtonPlug] = typeof(GitRepositoryButtonPlug).Name;
            _settingsMap[SettingsKeys.SecondButtonPlug] = typeof(NoneButtonPlug).Name;
            _settingsMap[SettingsKeys.ApplicationID] = DiscordRpcController.DefaultApplicationId;
            _settingsMap[SettingsKeys.UpdateTimeout] = (long) 1000;
            _settingsMap[SettingsKeys.Version] = "1.0.0";
            _settingsMap[SettingsKeys.UpdateNotifications] = true;
            _settingsMap[SettingsKeys.TranslationsPath] = "Translations/";
        }
    }
}
