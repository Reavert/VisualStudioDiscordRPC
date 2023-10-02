using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class ProjectNameTextPlug : BaseTextPlug
    {
        private readonly VsObserver _vsObserver;
        private readonly LocalizationService _localizationService;

        private Project _project;

        public ProjectNameTextPlug(VsObserver vsObserver, LocalizationService localizationService)
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
                return _localizationService.Localize(LocalizationKeys.NoActiveProject);
            }

            return string.Format("{0} {1}", _localizationService.Localize(LocalizationKeys.Project), _project.Name);
        }
    }
}
