using EnvDTE;
using LibGit2Sharp;
using System;
using System.IO;
using System.Linq;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class GitObserver
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

        private string _remoteUrl;

        public event Action<string> RemoteUrlChanged;

        public GitObserver(VsObserver vsObserver) 
        {
            vsObserver.SolutionChanged += OnSolutionChanged;
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
    }
}
