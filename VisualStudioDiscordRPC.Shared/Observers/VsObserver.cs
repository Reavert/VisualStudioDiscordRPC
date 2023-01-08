using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace VisualStudioDiscordRPC.Shared.Observers
{
    public class VsObserver : IObserver
    {
        public event DocumentChangedHandler DocumentChanged;
        public event ProjectChangedHandler ProjectChanged;
        public event SolutionChangedHandler SolutionChanged;

        private readonly DTE2 _dte;
        private string _lastSolutionName;

        public VsObserver(DTE2 dte)
        {
            _dte = dte;
        }

        public void Observe()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte.Events.WindowEvents.WindowActivated += OnWindowActivated;
        }

        public void Unobserve()
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            _dte.Events.WindowEvents.WindowActivated -= OnWindowActivated;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (gotFocus == lostFocus)
            {
                return;
            }

            Solution currentSolution = gotFocus.DTE.Solution;

            if (currentSolution?.FullName != _lastSolutionName)
            {
                _lastSolutionName = currentSolution.FullName;
                SolutionChanged?.Invoke(currentSolution);
            }
            
            if (gotFocus.Type == vsWindowType.vsWindowTypeDocument)
            {
                Project focusWindowProject = gotFocus?.Project;
                if (focusWindowProject != lostFocus?.Project)
                {
                    ProjectChanged?.Invoke(focusWindowProject);
                }

                Document focusWindowDocument = gotFocus?.Document;
                if (focusWindowDocument != lostFocus?.Document)
                {
                    DocumentChanged?.Invoke(focusWindowDocument);
                }
            }
            else
            {
                if (lostFocus == null)
                {
                    ProjectChanged?.Invoke(null);
                    DocumentChanged?.Invoke(null);
                }
            }
        }
    }
}
