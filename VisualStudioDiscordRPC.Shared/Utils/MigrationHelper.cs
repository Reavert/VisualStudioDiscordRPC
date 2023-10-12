using System;
using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class MigrationHelper
    {
        public static string GetAssetPlugNameFromLegacy(string legacyAssetPlugName, string defaultValue)
        {
            switch (legacyAssetPlugName)
            {
                case "NoneAssetSlot": return nameof(NoneAssetPlug);
                case "ExtensionIconSlot": return nameof(ExtensionIconPlug);
                case "VisualStudioVersionIconSlot": return nameof(VisualStudioVersionIconPlug);
                default: return defaultValue;
            }
        }

        public static string GetTextPlugNameFromLegacy(string legacyTextPlugName, string defaultValue)
        {
            switch (legacyTextPlugName)
            {
                case "NoneTextSlot": return nameof(NoneTextPlug);
                case "FileNameSlot": return nameof(FileNameTextPlug);
                case "ProjectNameSlot": return nameof(ProjectNameTextPlug);
                case "SolutionNameSlot": return nameof(SolutionNameTextPlug);
                case "VisualStudioVersionTextSlot": return nameof(VisualStudioVersionTextPlug);
                case "DebuggingSlot": return nameof(NoneTextPlug);
                default: return defaultValue;
            }
        }

        public static string GetTimerPlugNameFromLegacy(string legacyTimerPlugName, string defaulValue)
        {
            switch (legacyTimerPlugName)
            {
                case "NoneTimerSlot": return nameof(NoneTimerPlug);
                case "WithinFilesTimerSlot": return nameof(FileScopeTimerPlug);
                case "WithinProjectsTimerSlot": return nameof(ProjectScopeTimerPlug);
                case "WithinSolutionsTimerSlot": return nameof(SolutionScopeTimerPlug);
                case "WithinApplicationTimerSlot": return nameof(ApplicationScopeTimerPlug);
                default: return defaulValue;
            }
        }

        public static string GetButtonPlugNameFromLegacy(string legacyButtonPlugName, string defaultValue)
        {
            switch (legacyButtonPlugName)
            {
                case "NoneButtonSlot": return nameof(NoneButtonPlug);
                case "GitRepositoryButtonSlot": return nameof(GitRepositoryButtonPlug);
                default: return defaultValue;
            }
        }

        public static List<string> ListedSettingAsList(string listedSettingValue)
        {
            return listedSettingValue
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();
        }
    }
}
