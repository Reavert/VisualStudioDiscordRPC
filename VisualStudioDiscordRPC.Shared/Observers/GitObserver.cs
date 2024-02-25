using EnvDTE;
using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.Observers
{
    public class GitObserver : IObserver
    {
        public string RemoteUrl
        {
            get => _remoteUrl;
            private set
            {
                if (_remoteUrl == value)
                    return;

                _remoteUrl = value;
                RemoteUrlChanged?.Invoke(value);
            }
        }

        public string BranchName
        {
            get => _branchName;
            private set
            {
                if (_branchName == value)
                    return;

                _branchName = value;
                BranchNameChanged?.Invoke(value);
            }
        }

        public event Action<string> RemoteUrlChanged;
        public event Action<string> BranchNameChanged;

        private VsObserver _vsObserver;
        private Repository _lastRepository;

        private string _remoteUrl;
        private string _branchName;

        public GitObserver(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;
            OnWindowChanged(vsObserver.DTE.ActiveWindow);
            OnSolutionChanged(vsObserver.DTE.Solution);
        }

        private void OnWindowChanged(Window window)
        {
            UpdateBranchName();
        }

        private void OnSolutionChanged(Solution solution)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string repositoryPath = solution.FullName;
            if (string.IsNullOrEmpty(repositoryPath))
            {
                _lastRepository = null;
                return;
            }

            string repositoryName = Path.GetDirectoryName(repositoryPath);
            if (!Repository.IsValid(repositoryName))
            {
                _lastRepository = null;
                return;
            }

            _lastRepository = new Repository(repositoryName);
            UpdateRemoteUrl();
        }

        private void UpdateRemoteUrl()
        {
            if (_lastRepository == null)
            {
                RemoteUrl = string.Empty;
                return;
            }

            Remote firstRemote = _lastRepository.Network.Remotes.FirstOrDefault();
            if (firstRemote == null)
            {
                RemoteUrl = string.Empty;
                return;
            }

            RemoteUrl = firstRemote.Url;
        }

        private void UpdateBranchName()
        {
            if (_lastRepository == null)
            {
                BranchName = string.Empty;
                return;
            }

            BranchName = _lastRepository.Head.FriendlyName;
        }

        public void Observe()
        {
            _vsObserver.WindowChanged += OnWindowChanged;
            _vsObserver.SolutionChanged += OnSolutionChanged;
        }

        public void Unobserve()
        {
            _vsObserver.WindowChanged -= OnWindowChanged;
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }
    }
}
