using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs
{
    public class GitRepositoryButtonPlug : BaseButtonPlug
    {
        private readonly GitObserver _gitObserver;

        private string _remoteRepositoryUrl;
        private bool _isPrivateRepository;

        public bool HasRepository => !string.IsNullOrEmpty(_remoteRepositoryUrl);

        public bool IsPrivateRepository
        {
            get => _isPrivateRepository;
            set
            {
                _isPrivateRepository = value;
                Update();
            }
        }

        public GitRepositoryButtonPlug(GitObserver gitObserver) 
        {
            _gitObserver = gitObserver;
            _remoteRepositoryUrl = _gitObserver.RemoteUrl;
        }

        public override void Enable()
        {
            _gitObserver.RemoteUrlChanged += OnRemoteUrlChanged;
        }

        public override void Disable()
        {
            _gitObserver.RemoteUrlChanged -= OnRemoteUrlChanged;
        }

        private void OnRemoteUrlChanged(string newRemoteUrl)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _remoteRepositoryUrl = newRemoteUrl;
            Update();
        }

        protected override ButtonInfo GetData()
        {
            const string buttonName = "Repository";

            if (_isPrivateRepository || string.IsNullOrEmpty(_remoteRepositoryUrl))
            {
                return ButtonInfo.None;
            }

            return new ButtonInfo(buttonName, _remoteRepositoryUrl);
        }
    }
}
