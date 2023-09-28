using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class RepositorySecrecyService
    {
        private const string SecretRepositoriesFilename = "secret_repositories.json";

        private readonly GitObserver _gitObserver;
        private readonly GitRepositoryButtonSlot[] _gitRepositoryButtonSlots;
        private readonly string SecretRepositoriesFilePath;
        private readonly HashSet<string> _secretRepositories;

        private string _currentRepositoryRemoteUrl;

        public IReadOnlyCollection<string> SecretRepositories => _secretRepositories;

        public RepositorySecrecyService(GitObserver gitObserver, GitRepositoryButtonSlot[] buttons)
        {
            _gitObserver = gitObserver;
            _gitRepositoryButtonSlots = buttons;

            SecretRepositoriesFilePath = Path.Combine(PathHelper.GetApplicationDataPath(), SecretRepositoriesFilename);
            if (!File.Exists(SecretRepositoriesFilePath))
            {
                _secretRepositories = new HashSet<string>(1);
                return;
            }

            string json = File.ReadAllText(SecretRepositoriesFilePath);
            if (string.IsNullOrEmpty(json))
            {
                _secretRepositories = new HashSet<string>(1);
                return;
            }

            _secretRepositories = JsonConvert.DeserializeObject<HashSet<string>>(json);
            _currentRepositoryRemoteUrl = _gitObserver.RemoteUrl;
        }

        public void Start()
        {
            _gitObserver.RemoteUrlChanged += OnRemoteUrlChanged;
            SyncButtonsVisibilityStatus();
        }

        public void Stop()
        {
            _gitObserver.RemoteUrlChanged -= OnRemoteUrlChanged;
        }

        public void AddSecretRepository(string repositoryUrl)
        {
            if (string.IsNullOrEmpty(repositoryUrl))
                return;

            _secretRepositories.Add(repositoryUrl);

            SyncButtonsVisibilityStatus();
            SaveSecretRepositories();
        }

        public void RemoveSecretRepository(string repositoryUrl)
        {
            if (string.IsNullOrEmpty(repositoryUrl))
                return;

            _secretRepositories.Remove(repositoryUrl);

            SyncButtonsVisibilityStatus();
            SaveSecretRepositories();
        }

        public bool IsRepositorySecret(string repositoryUrl)
        {
            return _secretRepositories.Contains(repositoryUrl);
        }

        private void OnRemoteUrlChanged(string newRemoteUrl)
        {
            _currentRepositoryRemoteUrl = newRemoteUrl;
            SyncButtonsVisibilityStatus();
        }

        private void SyncButtonsVisibilityStatus()
        {
            bool isRepositorySecret = _secretRepositories.Contains(_currentRepositoryRemoteUrl);
            foreach (var gitRepositoryButtonSlot in _gitRepositoryButtonSlots)
            {
                gitRepositoryButtonSlot.IsPrivateRepository = isRepositorySecret;
            }
        }

        private void SaveSecretRepositories()
        {
            string json = JsonConvert.SerializeObject(_secretRepositories);
            File.WriteAllText(SecretRepositoriesFilePath, json);
        }
    }

    public class SecretRepositoriesCollectonProvider : IStringCollectionProvider
    {
        public IReadOnlyCollection<string> Items => _repositorySecrecyService.SecretRepositories;

        private readonly RepositorySecrecyService _repositorySecrecyService;

        public SecretRepositoriesCollectonProvider(RepositorySecrecyService repositorySecrecyService)
        {
            _repositorySecrecyService = repositorySecrecyService;
        }

        public void Add(string item)
        {
            _repositorySecrecyService.AddSecretRepository(item);
        }

        public void Remove(string item)
        {
            _repositorySecrecyService.RemoveSecretRepository(item);
        }
    }
}
