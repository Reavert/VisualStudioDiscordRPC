using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class SettingsHelper
    {
        private const char ListSeparator = '\n';

        public static void ClearSettings()
        {
            Settings.Default.Properties.Clear();
        }

        public static void SetSolutionSecret(string solutionPath, bool secret) 
            => SetSettingContent(nameof(Settings.HiddenSolutions), solutionPath, secret);

        public static bool IsSolutionSecret(string fullSolutionPath)
            => IsSettingContainsData(nameof(Settings.HiddenSolutions), fullSolutionPath);

        public static void SetRepositoryPrivate(string repositoryUrl, bool isPrivate)
            => SetSettingContent(nameof(Settings.PrivateRepositories), repositoryUrl, isPrivate);

        public static bool IsRepositoryPrivate(string fullSolutionPath)
            => IsSettingContainsData(nameof(Settings.PrivateRepositories), fullSolutionPath);

        private static void SetSettingContent(string settingName, string data, bool doContain)
        {
            var hashSetSetting = new ListedSetting(settingName);

            if (doContain)
            {
                if (!hashSetSetting.Contains(data))
                {
                    hashSetSetting.Add(data);
                }
            }
            else
            {
                hashSetSetting.Remove(data);
            }
        }

        private static bool IsSettingContainsData(string settingName, string data)
        {
            var listSetting = new ListedSetting(settingName);

            return listSetting.Contains(data);
        }

        public class ListedSetting
        {
            private readonly string _settingName;

            public ListedSetting(string settingName)
            {
                _settingName = settingName;
            }

            public void Add(string data)
            {
                List<string> list = ReadList();
                list.Add(data);

                WriteList(list);
            }

            public void Remove(string data)
            {
                List<string> list = ReadList();
                list.Remove(data);

                WriteList(list);
            }

            public bool Contains(string data)
            {
                List<string> stringList = ReadList();
                return stringList.Contains(data);
            }

            public List<string> GetItems()
            {
                return ReadList();
            }

            private List<string> ReadList()
            {
                var list = (string) Settings.Default[_settingName];
                return list
                    .Split(new[] { ListSeparator }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }

            private void WriteList(List<string> list)
            {
                Settings.Default[_settingName] = string.Join(ListSeparator.ToString(), list);
            }
        }
    }
}
