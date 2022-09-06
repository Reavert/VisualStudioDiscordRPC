using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared 
{
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal sealed class Settings : System.Configuration.ApplicationSettingsBase
    {
        private static Settings _defaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default => _defaultInstance;

        [System.Configuration.UserScopedSetting]
        [System.Configuration.DefaultSettingValue("true")]
        public string RichPresenceEnabled
        {
            get => (string)this["RichPresenceEnabled"];
            set => this["RichPresenceEnabled"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Configuration.DefaultSettingValue("English")]
        public string Language
        {
            get => (string)this["Language"];
            set => this["Language"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string LargeIcon
        {
            get => (string)this["LargeIcon"];
            set => this["LargeIcon"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string SmallIcon
        {
            get => (string)this["SmallIcon"];
            set => this["SmallIcon"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string TitleText
        {
            get => (string)this["TitleText"];
            set => this["TitleText"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string SubTitleText
        {
            get => (string)this["SubTitleText"];
            set => this["SubTitleText"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string WorkTimerMode
        {
            get => (string)this["WorkTimerMode"];
            set => this["WorkTimerMode"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        public string GitLinkVisible
        {
            get => (string)this["GitLinkVisible"];
            set => this["GitLinkVisible"] = value;
        }

        [System.Configuration.UserScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Configuration.DefaultSettingValue("false")]
        public bool Updated
        {
            get => (bool)this["Updated"];
            set => this["Updated"] = value;
        }

        [System.Configuration.ApplicationScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Configuration.DefaultSettingValue("914622396630175855")]
        public string ApplicationID => (string)this["ApplicationID"];

        [System.Configuration.ApplicationScopedSetting]
        [System.Diagnostics.DebuggerNonUserCode]
        [System.Configuration.DefaultSettingValue("Translations/")]
        public string TranslationsPath => (string)this["TranslationsPath"];
    }
}
