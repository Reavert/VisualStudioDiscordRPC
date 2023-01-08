using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinSolutionsTimerSlot : TimerSlot
    {
        private VsObserver _vsObserver;

        public WithinSolutionsTimerSlot(VsObserver vsObserver)
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
            Update();
        }
    }
}
