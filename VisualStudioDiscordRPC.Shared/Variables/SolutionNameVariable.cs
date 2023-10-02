using EnvDTE;
using System.IO;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class SolutionNameVariable : Variable
    {
        private Solution _solution;

        public SolutionNameVariable(VsObserver vsObserver) 
        {
            vsObserver.SolutionChanged += OnSolutionChanged;
            _solution = vsObserver.DTE.Solution;
        }

        public override string GetData()
        {
            return Path.GetFileNameWithoutExtension(_solution?.FullName);
        }

        private void OnSolutionChanged(Solution solution)
        {
            _solution = solution;
            RaiseChangedEvent();
        }
    }
}
