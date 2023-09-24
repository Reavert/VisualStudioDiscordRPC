using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
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
                PopulateFromOldSettings();
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

        public List<T> ReadList<T>(string key)
        {
            if (_settingsMap.TryGetValue(key, out var value))
            {
                return new List<T>(0);
            }

            return JsonConvert.DeserializeObject<List<T>>(value.ToString());
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

        private void PopulateFromOldSettings()
        {
            _settingsMap[SettingsKeys.RichPresenceEnabled] = bool.Parse(Settings.Default.RichPresenceEnabled);
            _settingsMap[SettingsKeys.Language] = Settings.Default.Language;
            _settingsMap[SettingsKeys.LargeIconSlot] = Settings.Default.LargeIconSlot;
            _settingsMap[SettingsKeys.SmallIconSlot] = Settings.Default.SmallIconSlot;
            _settingsMap[SettingsKeys.TimerSlot] = Settings.Default.TimerSlot;
            _settingsMap[SettingsKeys.FirstButtonSlot] = Settings.Default.FirstButtonSlot;
            _settingsMap[SettingsKeys.SecondButtonSlot] = Settings.Default.SecondButtonSlot;
            _settingsMap[SettingsKeys.Updated] = Settings.Default.Updated;
            _settingsMap[SettingsKeys.ApplicationID] = Settings.Default.ApplicationID;
            _settingsMap[SettingsKeys.UpdateTimeout] = int.Parse(Settings.Default.UpdateTimeout);
            _settingsMap[SettingsKeys.Version] = Settings.Default.Version;
            _settingsMap[SettingsKeys.UpdateNotifications] = bool.Parse(Settings.Default.UpdateNotifications);
            _settingsMap[SettingsKeys.TranslationsPath] = Settings.Default.TranslationsPath;

            var hiddenSolutions = new SettingsHelper.ListedSetting(nameof(Settings.Default.HiddenSolutions));
            _settingsMap["HiddenSolutions"] = hiddenSolutions.GetItems();

            var privateRepositories = new SettingsHelper.ListedSetting(nameof(Settings.Default.PrivateRepositories));
            _settingsMap["PrivateRepositories"] = privateRepositories.GetItems();            
        }
    }
}
