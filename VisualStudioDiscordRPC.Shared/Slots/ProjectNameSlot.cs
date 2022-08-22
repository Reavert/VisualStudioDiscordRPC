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

        public ProjectNameSlot(IObserver observer) : base(observer)
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        public override void Enable()
        {
            Observer.ProjectChanged += OnProjectChanged;
        }

        public override void Disable()
        {
            Observer.ProjectChanged -= OnProjectChanged;
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
