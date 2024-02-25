using System;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class GitBranchTextPlug : BaseTextPlug
    {
        private readonly LocalizationService _localizationService;
        private readonly GitObserver _gitObserver;

        private string _branchName;

        public GitBranchTextPlug(GitObserver gitObserver, LocalizationService localizationService)
        {
            _gitObserver = gitObserver;
            _localizationService = localizationService;

            _branchName = gitObserver.BranchName;
        }

        public override void Enable()
        {
            _gitObserver.BranchNameChanged += OnBranchNameChanged;
        }

        public override void Disable()
        {
            _gitObserver.BranchNameChanged -= OnBranchNameChanged;
        }

        private void OnBranchNameChanged(string newBranchName)
        {
            _branchName = newBranchName;
            Update();
        }

        protected override string GetData()
        {
            if (string.IsNullOrEmpty(_branchName))
            {
                return _localizationService.Localize(LocalizationKeys.NoActiveBranch);
            }

            return $"{_localizationService.Localize(LocalizationKeys.Branch)} {_branchName}";
        }
    }
}
