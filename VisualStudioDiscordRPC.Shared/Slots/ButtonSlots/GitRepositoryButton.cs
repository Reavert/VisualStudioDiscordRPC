using EnvDTE;
using LibGit2Sharp;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.ButtonSlots
{
    public class GitRepositoryButton : ButtonSlot
    {
        private readonly VsObserver _vsObserver;

        private string _remoteRepositoryUrl;

        public GitRepositoryButton(VsObserver vsObserver) 
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

            _remoteRepositoryUrl = GetValidRemoteGitUrlOrSolution(solutionPath);
            Update();
        }

        private string GetValidRemoteGitUrlOrSolution(string repositoryPath)
        {
            if (!string.IsNullOrEmpty(repositoryPath))
            {
                string repositoryName = Path.GetDirectoryName(repositoryPath);
                if (Repository.IsValid(repositoryName))
                {
                    Remote firstRemote = new Repository(repositoryName).Network.Remotes.FirstOrDefault();

                    if (firstRemote != null)
                    {
                        return firstRemote.Url;
                    }
                }
            }

            return null;
        }

        protected override ButtonInfo GetData()
        {
            const string buttonName = "Repository";

            if (string.IsNullOrEmpty(_remoteRepositoryUrl))
            {
                return ButtonInfo.None;
            }

            return new ButtonInfo(buttonName, _remoteRepositoryUrl);
        }
    }
}
