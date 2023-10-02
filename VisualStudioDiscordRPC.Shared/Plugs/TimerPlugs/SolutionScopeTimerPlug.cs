using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public class SolutionScopeTimerPlug : BaseTimerPlug
    {
        private readonly VsObserver _vsObserver;

        public SolutionScopeTimerPlug(VsObserver vsObserver) : base()
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
            SyncTimestamp();
        }
    }
}
