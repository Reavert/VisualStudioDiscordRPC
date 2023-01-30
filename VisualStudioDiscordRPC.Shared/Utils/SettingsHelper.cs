using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class SettingsHelper
    {
        private const char SolutionHashesSeparator = ';';

        private static readonly SHA256 _sha256 = SHA256.Create();

        public static void SetSolutionVisible(string fullSolutionPath, bool visible)
        {
            List<string> hashes = ReadHiddenSolutionsHashes();
            string solutionHash = Hash(fullSolutionPath);

            if (visible)
            {
                hashes.Remove(solutionHash);
            }
            else
            {
                if (!hashes.Contains(solutionHash))
                {
                    hashes.Add(solutionHash);
                }
            }

            WriteHiddenSolutionHashes(hashes);
        }

        public static bool IsSolutionVisible(string fullSolutionPath)
        {
            List<string> hashes = ReadHiddenSolutionsHashes();
            string solutionHash = Hash(fullSolutionPath);

            return !hashes.Contains(solutionHash);
        }

        private static List<string> ReadHiddenSolutionsHashes()
        {
            return Settings.Default.HiddenSolutions.Split(SolutionHashesSeparator).ToList();

        }

        private static void WriteHiddenSolutionHashes(List<string> hashes)
        {
            Settings.Default.HiddenSolutions = string.Join(SolutionHashesSeparator.ToString(), hashes);
        }

        private static string Hash(string data)
        {
            byte[] hashedData = _sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Encoding.UTF8.GetString(hashedData);
        }
    }
}
