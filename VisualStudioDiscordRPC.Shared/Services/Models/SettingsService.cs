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
                FileStream fileStream = File.Create(SettingsPath);
                fileStream.Close();

                PopulateFromOldSettings();

                Save();
            }
            else
            {
                string json = File.ReadAllText(SettingsPath);
                _settingsMap = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            }

        }

        public T Read<T>(string key) where T : class
        {
            _settingsMap.TryGetValue(key, out var value);
            return value as T;
        }

        public void Set<T>(string key, T value)
        {
            _settingsMap[key] = value;
        }

        public void Save()
        {
            var settingsJson = JsonConvert.SerializeObject(_settingsMap);
            File.WriteAllText(settingsJson);
        }

        private void PopulateFromOldSettings()
        {
            _settingsMap["RichPresenceEnabled"] = Settings.Default.RichPresenceEnabled;
            _settingsMap["Language"] = Settings.Default.Language;
            _settingsMap["LargeIconSlot"] = Settings.Default.LargeIconSlot;
            _settingsMap["SmallIconSlot"] = Settings.Default.SmallIconSlot;
            _settingsMap["TimerSlot"] = Settings.Default.TimerSlot;
            _settingsMap["FirstButtonSlot"] = Settings.Default.FirstButtonSlot;
            _settingsMap["SecondButtonSlot"] = Settings.Default.SecondButtonSlot;
            _settingsMap["Updated"] = Settings.Default.Updated;
            _settingsMap["ApplicationID"] = Settings.Default.ApplicationID;
            _settingsMap["UpdateTimeout"] = Settings.Default.UpdateTimeout;
            _settingsMap["Version"] = Settings.Default.Version;
            _settingsMap["UpdateNotifications"] = Settings.Default.UpdateNotifications;
            _settingsMap["TranslationsPath"] = Settings.Default.TranslationsPath;

            var hiddenSolutions = new SettingsHelper.ListedSetting(nameof(Settings.Default.HiddenSolutions));
            _settingsMap["HiddenSolutions"] = hiddenSolutions.GetItems();

            var privateRepositories = new SettingsHelper.ListedSetting(nameof(Settings.Default.PrivateRepositories));
            _settingsMap["PrivateRepositories"] = privateRepositories.GetItems();            
        }
    }
}
