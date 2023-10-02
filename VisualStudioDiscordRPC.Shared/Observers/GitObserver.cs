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
                _remoteUrl = value;
                RemoteUrlChanged?.Invoke(value);
            }
        }

        public event Action<string> RemoteUrlChanged;

        private VsObserver _vsObserver;
        private string _remoteUrl;

        public GitObserver(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;
            OnSolutionChanged(vsObserver.DTE.Solution);
        }

        private void OnSolutionChanged(Solution solution)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string repositoryPath = solution.FullName;
            if (string.IsNullOrEmpty(repositoryPath))
            {
                RemoteUrl = string.Empty;
                return;
            }

            string repositoryName = Path.GetDirectoryName(repositoryPath);
            if (!Repository.IsValid(repositoryName))
            {
                RemoteUrl = string.Empty;
                return;
            }

            Remote firstRemote = new Repository(repositoryName).Network.Remotes.FirstOrDefault();
            if (firstRemote == null)
            {
                RemoteUrl = string.Empty;
                return;
            }

            RemoteUrl = firstRemote.Url;
        }

        public void Observe()
        {
            _vsObserver.SolutionChanged += OnSolutionChanged;
        }

        public void Unobserve()
        {
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }
    }
}
