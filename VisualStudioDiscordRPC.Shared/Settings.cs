namespace VisualStudioDiscordRPC.Shared 
{
    [System.Runtime.CompilerServices.CompilerGenerated()]
    internal sealed partial class Settings : System.Configuration.ApplicationSettingsBase {

        private static Settings _defaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default {
            get {
                return _defaultInstance;
            }
        }

        [System.Configuration.UserScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.DefaultSettingValue("English")]
        public string Language {
            get => (string)this["Language"];
            set => this["Language"] = value;
        }
    }
}
