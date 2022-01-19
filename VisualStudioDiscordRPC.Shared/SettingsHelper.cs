using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared
{
    public class SettingsHelper
    {
        public EnumStringMap<RichPresenceWrapper.Text> TextEnumMap;
        public EnumStringMap<RichPresenceWrapper.Icon> IconEnumMap;
        public EnumStringMap<RichPresenceWrapper.TimerMode> TimerModeEnumMap;

        private static SettingsHelper _instance;
        public static SettingsHelper Instance => _instance ?? (_instance = new SettingsHelper());

        internal SettingsHelper()
        {
            TextEnumMap = new EnumStringMap<RichPresenceWrapper.Text>();
            IconEnumMap = new EnumStringMap<RichPresenceWrapper.Icon>();
            TimerModeEnumMap = new EnumStringMap<RichPresenceWrapper.TimerMode>();
        }
    }
}
