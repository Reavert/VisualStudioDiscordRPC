using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared
{
    public class SettingsHelper
    {
        private static Dictionary<RichPresenceWrapper.Text, string> _textOptions;

        private static Dictionary<RichPresenceWrapper.Icon, string> _iconOptions;

        private static SettingsHelper _instance;
        public static SettingsHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    return new SettingsHelper();
                }

                return _instance;
            }
        }

        internal SettingsHelper()
        {
            var textEnumValues = Enum.GetValues(typeof(RichPresenceWrapper.Text));
            var iconEnumValues = Enum.GetValues(typeof(RichPresenceWrapper.Icon));

            _textOptions = new Dictionary<RichPresenceWrapper.Text, string>();
            _iconOptions = new Dictionary<RichPresenceWrapper.Icon, string>();

            foreach (RichPresenceWrapper.Text textEnumValue in textEnumValues)
            {
                _textOptions.Add(textEnumValue, textEnumValue.ToString());
            }

            foreach (RichPresenceWrapper.Icon iconEnumValue in iconEnumValues)
            {
                _iconOptions.Add(iconEnumValue, iconEnumValue.ToString());
            }
        }

        public string GetStringFromTextOption(RichPresenceWrapper.Text textOption)
        {
            return _textOptions[textOption];
        }

        public RichPresenceWrapper.Text GetTextOptionFromString(string value)
        {
            return _textOptions.FirstOrDefault(textOption => textOption.Value == value).Key;
        }

        public string GetStringFromIconOption(RichPresenceWrapper.Icon iconOption)
        {
            return _iconOptions[iconOption];
        }

        public RichPresenceWrapper.Icon GetIconOptionFromString(string value)
        {
            return _iconOptions.FirstOrDefault(iconOption => iconOption.Value == value).Key;
        }
    }
}
