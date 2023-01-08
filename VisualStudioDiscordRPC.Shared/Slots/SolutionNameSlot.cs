using EnvDTE;
using System.IO;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class SolutionNameSlot : TextSlot
    {
        private VsObserver _vsObserver;
        private LocalizationService<LocalizationFile> _localizationService;

        private string _solutionName;

        public SolutionNameSlot(VsObserver vsObserver) 
        {
            _vsObserver = vsObserver;
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        public override void Enable()
        {
            _vsObserver.SolutionChanged += OnSolutionChanged;
        }

        public override void Disable()
        {
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }

        private void OnSolutionChanged(Solution solution)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (solution == null)
            {
                _solutionName = null;
                return;
            }

            _solutionName = Path.GetFileNameWithoutExtension(solution.FullName);
            Update();
        }

        protected override string GetData()
        {
            if (string.IsNullOrEmpty(_solutionName))
            {
                return _localizationService.Current.NoActiveSolution;
            }

            return string.Format(ConstantStrings.ActiveSolutionFormat, _localizationService.Current.Solution, _solutionName);
        }
    }
}
