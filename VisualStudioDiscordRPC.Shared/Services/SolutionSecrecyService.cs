using EnvDTE;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services
{
    public class SolutionSecrecyService
    {
        private const string SecretSolutionsFilename = "secret_solutions.json";

        private readonly VsObserver _vsObserver;
        private readonly DiscordRpcController _discordRpcController;
        private readonly HashSet<string> _secretSolutions;
        private readonly string SecretSolutionsFilePath;

        private Solution _lastOpenedSolution;

        public IReadOnlyCollection<string> SecretSolutions => _secretSolutions;

        public SolutionSecrecyService(VsObserver vsObserver, DiscordRpcController discordRpcController)
        {
            _vsObserver = vsObserver;
            _discordRpcController = discordRpcController;
            _lastOpenedSolution = _vsObserver.DTE.Solution;

            SecretSolutionsFilePath = Path.Combine(PathHelper.GetApplicationDataPath(), SecretSolutionsFilename);

            if (!File.Exists(SecretSolutionsFilePath))
            {
                // Migrate from legacy settings.
                _secretSolutions = MigrationHelper.ListedSettingAsList(Settings.Default.HiddenSolutions).ToHashSet();
                SaveSecretSolutions();
                return;
            }

            string json = File.ReadAllText(SecretSolutionsFilePath);
            if (string.IsNullOrEmpty(json)) 
            {
                _secretSolutions = new HashSet<string>(1);
                return;
            }

            _secretSolutions = JsonConvert.DeserializeObject<HashSet<string>>(json);
        }

        public void Start()
        {
            _vsObserver.SolutionChanged += OnSolutionChanged;
            SyncRpcSecrecyStatus();
        }

        public void Stop()
        {
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }

        public void AddSecretSolution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
                return;

            _secretSolutions.Add(solutionPath);

            SyncRpcSecrecyStatus();
            SaveSecretSolutions();
        }

        public void RemoveSecretSolution(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
                return;

            _secretSolutions.Remove(solutionPath);
            
            SyncRpcSecrecyStatus();
            SaveSecretSolutions();
        }

        public bool IsSolutionSecret(string solutionPath)
        {
            if (string.IsNullOrEmpty(solutionPath))
                return false;

            // Check for exact match first
            if (_secretSolutions.Contains(solutionPath))
                return true;

            // Check for partial matches - if solution path starts with any of the secret paths
            return _secretSolutions.Any(secretPath => PathHelper.IsPathBaseOf(secretPath, solutionPath));
        }


        private void OnSolutionChanged(Solution solution)
        {
            _lastOpenedSolution = solution;
            SyncRpcSecrecyStatus();
        }

        private void SyncRpcSecrecyStatus()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string fullSolutionName = _lastOpenedSolution?.FullName;
            _discordRpcController.Secret = IsSolutionSecret(fullSolutionName);
        }

        private void SaveSecretSolutions()
        {
            string json = JsonConvert.SerializeObject(_secretSolutions);
            File.WriteAllText(SecretSolutionsFilePath, json);
        }
    }

    public class SecretSolutionsCollectionProvider : IStringCollectionProvider
    {
        public IReadOnlyCollection<string> Items => _solutionSecrecyService.SecretSolutions;

        private readonly SolutionSecrecyService _solutionSecrecyService;

        public SecretSolutionsCollectionProvider(SolutionSecrecyService solutionSecrecyService)
        {
            _solutionSecrecyService = solutionSecrecyService;
        }

        public void Add(string item)
        {
            _solutionSecrecyService.AddSecretSolution(item);
        }

        public void Remove(string item)
        {
            _solutionSecrecyService.RemoveSecretSolution(item);
        }
    }
}
