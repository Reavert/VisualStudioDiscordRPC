using EnvDTE;
using LibGit2Sharp;
using System.IO;
using System.Linq;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Slots.ButtonSlots
{
    public class GitRepositoryButtonSlot : ButtonSlot
    {
        private readonly VsObserver _vsObserver;

        private string _remoteRepositoryUrl;

        public bool HasRepository => !string.IsNullOrEmpty(_remoteRepositoryUrl);

        public bool IsPrivateRepository
        {
            get
            {
                if (string.IsNullOrEmpty(_remoteRepositoryUrl))
                {
                    return false;
                }

                return SettingsHelper.IsRepositoryPrivate(_remoteRepositoryUrl);
            }
            set
            {
                if (string.IsNullOrEmpty(_remoteRepositoryUrl))
                {
                    return;
                }

                SettingsHelper.SetRepositoryPrivate(_remoteRepositoryUrl, value);
                Update();
            } 
        }

        public GitRepositoryButtonSlot(VsObserver vsObserver) 
        {
            _vsObserver = vsObserver;
        }

        public override void Enable()
        {
            _vsObserver.SolutionChanged += OnSolutionChanged;
        }

        public override void Disable()
        {
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }

        private void OnSolutionChanged(Solution solution)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (solution == null)
            {
                _remoteRepositoryUrl = null;
                return;
            }

            string solutionPath = solution.FullName;
            _remoteRepositoryUrl = GetValidRemoteGitUrl(solutionPath);

            Update();
        }

        private string GetValidRemoteGitUrl(string repositoryPath)
        {
            if (string.IsNullOrEmpty(repositoryPath))
            {
                return null;
            }

            string repositoryName = Path.GetDirectoryName(repositoryPath);
            if (!Repository.IsValid(repositoryName))
            {
                return null;
            }

            Remote firstRemote = new Repository(repositoryName).Network.Remotes.FirstOrDefault();
            if (firstRemote == null)
            {
                return null;
            }

            return firstRemote.Url;
        }

        protected override ButtonInfo GetData()
        {
            const string buttonName = "Repository";

            if (IsPrivateRepository || string.IsNullOrEmpty(_remoteRepositoryUrl))
            {
                return ButtonInfo.None;
            }

            return new ButtonInfo(buttonName, _remoteRepositoryUrl);
        }
    }
}
