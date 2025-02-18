using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;

namespace VisualStudioDiscordRPC.Shared.Observers
{
    public class VsObserver : IObserver
    {
        public event WindowChangedHandler WindowChanged;
        public event DocumentChangedHandler DocumentChanged;
        public event ProjectChangedHandler ProjectChanged;
        public event SolutionChangedHandler SolutionChanged;
        public event TextEditorLineChangedHandler TextEditorLineChanged;

        private readonly DTE2 _dte;
        public DTE2 DTE => _dte;

        private string _lastSolutionName;
        private Project _lastProject;

        private TextEditorEvents _textEditorEvents;

        public VsObserver(DTE2 dte)
        {
            _dte = dte;
        }

        public void Observe()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _dte.Events.WindowEvents.WindowActivated += OnWindowActivated;

            _textEditorEvents = _dte.Events.TextEditorEvents;
            _textEditorEvents.LineChanged += OnTextEditorLineChanged;
        }

        public void Unobserve()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _dte.Events.WindowEvents.WindowActivated -= OnWindowActivated;
            _textEditorEvents.LineChanged -= OnTextEditorLineChanged;
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (gotFocus == lostFocus)
            {
                return;
            }

            WindowChanged?.Invoke(gotFocus);

            Solution currentSolution = gotFocus.DTE.Solution;

            if (currentSolution?.FullName != _lastSolutionName)
            {
                _lastSolutionName = currentSolution.FullName;
                SolutionChanged?.Invoke(currentSolution);
            }
            
            if (gotFocus.Type == vsWindowType.vsWindowTypeDocument)
            {
                Document focusWindowDocument = gotFocus?.Document;
                if (focusWindowDocument != lostFocus?.Document)
                {
                    DocumentChanged?.Invoke(focusWindowDocument);
                }

                if (gotFocus.Project != _lastProject)
                {
                    _lastProject = gotFocus.Project;
                    ProjectChanged?.Invoke(gotFocus.Project);
                }
            }
            else
            {
                if (lostFocus == null)
                {
                    DocumentChanged?.Invoke(null);
                }
            }
        }

        private void OnTextEditorLineChanged(TextPoint startPoint, TextPoint endPoint, int hint)
        {
            TextEditorLineChanged?.Invoke(startPoint, endPoint, hint);
        }
    }
}
