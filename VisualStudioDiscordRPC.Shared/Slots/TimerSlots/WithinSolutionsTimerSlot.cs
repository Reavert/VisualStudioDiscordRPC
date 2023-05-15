using DiscordRPC;
using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinSolutionsTimerSlot : TimerSlot
    {
        private readonly VsObserver _vsObserver;

        public WithinSolutionsTimerSlot(VsObserver vsObserver) : base()
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
            ChangeTimestamp = Timestamps.Now;
        }
    }
}
