using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;

namespace VisualStudioDiscordRPC.Shared.Observers
{
    public class VsObserver : IObserver
    {
        public event DocumentChangedHandler DocumentChanged;
        public event ProjectChangedHandler ProjectChanged;
        public event SolutionChangedHandler SolutionChanged;

        private readonly DTE2 _dte;
        private Solution _solution;

        public VsObserver(DTE2 dte)
        {
            _dte = dte;
            _solution = _dte.Solution;
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

            if (_solution != _dte.Solution)
            {
                _solution = _dte.Solution;
                SolutionChanged?.Invoke(_solution);
            }

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
    }
}
