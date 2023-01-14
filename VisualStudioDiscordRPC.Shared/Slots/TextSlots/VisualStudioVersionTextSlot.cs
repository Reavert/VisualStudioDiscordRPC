using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class VisualStudioVersionTextSlot : TextSlot
    {
        private readonly VsObserver _vsObserver;

        private string _edition;
        private string _version;

        public VisualStudioVersionTextSlot(VsObserver vsObserver)
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

            _edition = solution.DTE.Edition;
            _version = VisualStudioHelper.GetVersionByDevNumber(solution.DTE.Version);

            Update();
        }

        protected override string GetData()
        {
            return string.Format(ConstantStrings.VisualStudioVersion, _edition, _version);
        }
    }
}
