using DiscordRPC;
using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TimerSlots
{
    public class WithinProjectsTimerSlot : TimerSlot
    {
        private readonly VsObserver _vsObserver;

        public WithinProjectsTimerSlot(VsObserver vsObserver) : base()
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
