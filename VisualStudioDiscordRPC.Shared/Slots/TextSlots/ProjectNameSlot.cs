using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class ProjectNameSlot : TextSlot
    {
        private readonly LocalizationService<LocalizationFile> _localizationService;
        private readonly VsObserver _vsObserver;

        private Project _project;

        public ProjectNameSlot(VsObserver vsObserver)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            _vsObserver = vsObserver;
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
                return _localizationService.Current.NoActiveProject;
            }

            return string.Format(ConstantStrings.ActiveProjectFormat, _localizationService.Current.Project, _project.Name);
        }
    }
}
