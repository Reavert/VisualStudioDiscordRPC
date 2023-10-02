using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public class ProjectScopeTimerPlug : BaseTimerPlug
    {
        private readonly VsObserver _vsObserver;

        public ProjectScopeTimerPlug(VsObserver vsObserver) : base()
        {
            _vsObserver = vsObserver;
        }

        public override void Enable()
        {
            _vsObserver.ProjectChanged += OnProjectChanged;
        }

        public override void Disable()
        {
            _vsObserver.ProjectChanged -= OnProjectChanged;
        }

        private void OnProjectChanged(Project project)
        {
            SyncTimestamp();
        }
    }
}
