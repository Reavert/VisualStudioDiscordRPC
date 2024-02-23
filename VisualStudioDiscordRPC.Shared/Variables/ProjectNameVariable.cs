using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class ProjectNameVariable : Variable
    {
        private readonly VsObserver _vsObserver;

        private Project _project;

        public ProjectNameVariable(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;   
        }

        public override void Initialize()
        {
            _project = _vsObserver.DTE.ActiveWindow?.Project;
            _vsObserver.ProjectChanged += OnProjectChanged;
        }

        public override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return _project?.Name;
        }

        private void OnProjectChanged(Project project)
        {
            _project = project;
            RaiseChangedEvent();
        }
    }
}
