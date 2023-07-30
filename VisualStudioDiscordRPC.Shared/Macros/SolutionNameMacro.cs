using EnvDTE;
using System;
using System.IO;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public class SolutionNameMacro : Macro
    {
        private Solution _solution;

        public SolutionNameMacro(VsObserver vsObserver) 
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
