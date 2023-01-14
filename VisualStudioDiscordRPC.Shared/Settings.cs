using System.Configuration;
using System.Reflection;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;

namespace VisualStudioDiscordRPC.Shared
{
    [System.Runtime.CompilerServices.CompilerGenerated]
    internal sealed class Settings : ApplicationSettingsBase
    {
        private static Settings _defaultInstance = (Settings)Synchronized(new Settings());

        public static Settings Default => _defaultInstance;

        [UserScopedSetting]
        [DefaultSettingValue("true")]
        public string RichPresenceEnabled
        {
            get => (string)this[nameof(RichPresenceEnabled)];
            set => this[nameof(RichPresenceEnabled)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("English")]
        public string Language
        {
            get => (string)this[nameof(Language)];
            set => this[nameof(Language)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(ExtensionIconSlot))]
        public string LargeIconSlot
        {
            get => (string)this[nameof(LargeIconSlot)];
            set => this[nameof(LargeIconSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(VisualStudioVersionIconSlot))]
        public string SmallIconSlot
        {
            get => (string)this[nameof(SmallIconSlot)];
            set => this[nameof(SmallIconSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(FileNameSlot))]
        public string DetailsSlot
        {
            get => (string)this[nameof(DetailsSlot)];
            set => this[nameof(DetailsSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(SolutionNameSlot))]
        public string StateSlot
        {
            get => (string)this[nameof(StateSlot)];
            set => this[nameof(StateSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(WithinFilesTimerSlot))]
        public string TimerSlot
        {
            get => (string)this[nameof(TimerSlot)];
            set => this[nameof(TimerSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(GitRepositoryButtonSlot))]
        public string FirstButtonSlot
        {
            get => (string)this[nameof(FirstButtonSlot)];
            set => this[nameof(FirstButtonSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue(nameof(NoneButtonSlot))]
        public string SecondButtonSlot
        {
            get => (string)this[nameof(SecondButtonSlot)];
            set => this[nameof(SecondButtonSlot)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("false")]
        public bool Updated
        {
            get => (bool)this[nameof(Updated)];
            set => this[nameof(Updated)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("914622396630175855")]
        public string ApplicationID
        {
            get => (string)this[nameof(ApplicationID)];
            set => this[nameof(ApplicationID)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("1000")]
        public string UpdateTimeout
        {
            get => (string)this[nameof(UpdateTimeout)];
            set => this[nameof(UpdateTimeout)] = value;
        }

        [ApplicationScopedSetting]
        [DefaultSettingValue("Translations/")]
        public string TranslationsPath => (string)this["TranslationsPath"];
    }
}
