using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class ProjectNameTextSlot : TextSlot
    {
        private readonly VsObserver _vsObserver;
        private readonly LocalizationService<LocalizationFile> _localizationService;

        private Project _project;

        public ProjectNameTextSlot(VsObserver vsObserver, LocalizationService<LocalizationFile> localizationService)
        {
            _vsObserver = vsObserver;
            _localizationService = localizationService;

            _project = _vsObserver.DTE.ActiveWindow?.Project;
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
            _project = project;
            Update();
        }

        protected override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (_project == null)
            {
                return _localizationService.Current.NoActiveSolution;
            }

            return string.Format("{0} {1}", _localizationService.Current.Project, _project.Name);
        }
    }
}
