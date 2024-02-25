using Microsoft.VisualStudio.RpcContracts.Commands;
using System;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class GitBranchVariable : Variable
    {
        private readonly GitObserver _gitObserver;
        private string _branchName;

        public GitBranchVariable(GitObserver gitObserver)
        {
            _gitObserver = gitObserver;
        }

        public override void Initialize()
        {
            _gitObserver.BranchNameChanged += OnBranchNameChanged;
        }

        private void OnBranchNameChanged(string newBranchName)
        {
            _branchName = newBranchName;
            RaiseChangedEvent();
        }

        public override string GetData()
        {
            return string.IsNullOrEmpty(_branchName) ? "none" : _branchName;
        }
    }
}
