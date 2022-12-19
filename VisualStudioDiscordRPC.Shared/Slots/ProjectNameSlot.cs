using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class ProjectNameSlot : AbstractSlot
    {
        private LocalizationService<LocalizationFile> _localizationService;
        private VsObserver _vsObserver;

        public ProjectNameSlot(VsObserver vsObserver)
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
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
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string projectNameText;
            if (project != null)
            {
                projectNameText =
                    string.Format(ConstantStrings.ActiveProjectFormat, _localizationService.Current.Project, project.Name);
            }
            else
            {
                projectNameText = _localizationService.Current.NoActiveProject;
            }

            PerformUpdate(projectNameText);
        }
    }
}
