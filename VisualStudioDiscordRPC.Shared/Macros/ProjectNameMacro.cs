using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public class ProjectNameMacro : Macro
    {
        private Project _project;

        public ProjectNameMacro(VsObserver vsObserver)
        {
            _project = vsObserver.DTE.ActiveWindow?.Project;
            vsObserver.ProjectChanged += OnProjectChanged;
        }

        public override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return _project.Name;
        }

        private void OnProjectChanged(Project project)
        {
            _project = project;
            RaiseChangedEvent();
        }
    }
}
