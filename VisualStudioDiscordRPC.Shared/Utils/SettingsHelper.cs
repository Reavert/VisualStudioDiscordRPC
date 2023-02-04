using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class SettingsHelper
    {
        private const char SolutionHashesSeparator = ';';

        private static readonly MD5 _md5 = MD5.Create();

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
            var hashSetSetting = new HashSetSetting(settingName);

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
            var hashSetSetting = new HashSetSetting(settingName);

            return hashSetSetting.Contains(data);
        }

        public class HashSetSetting
        {
            private readonly string _settingName;

            public HashSetSetting(string settingName)
            {
                _settingName = settingName;
            }

            public void Add(string data)
            {
                List<string> hashes = ReadHashes();
                
                string dataHash = Hash(data);
                hashes.Add(dataHash);

                WriteHashes(hashes);
            }

            public void Remove(string data)
            {
                List<string> hashes = ReadHashes();

                string dataHash = Hash(data);
                hashes.Remove(dataHash);

                WriteHashes(hashes);
            }

            public bool Contains(string data)
            {
                List<string> hashes = ReadHashes();
                string solutionHash = Hash(data);

                return hashes.Contains(solutionHash);
            }

            private List<string> ReadHashes()
            {
                var hashes = (string) Settings.Default[_settingName];
                return hashes.Split(SolutionHashesSeparator).ToList();

            }

            private void WriteHashes(List<string> hashes)
            {
                Settings.Default[_settingName] = string.Join(SolutionHashesSeparator.ToString(), hashes);
            }

            private string Hash(string data)
            {
                byte[] hash = _md5.ComputeHash(Encoding.ASCII.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
