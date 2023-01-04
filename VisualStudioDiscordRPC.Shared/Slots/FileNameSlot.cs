using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class FileNameSlot : TextSlot
    {
        private LocalizationService<LocalizationFile> _localizationService;
        private VsObserver _vsObserver;

        public FileNameSlot(VsObserver vsObserver)
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            _vsObserver = vsObserver;
        }

        public override void Enable()
        {
            _vsObserver.DocumentChanged += OnDocumentChanged;
        }

        public override void Disable()
        {
            _vsObserver.DocumentChanged -= OnDocumentChanged;
        }

        private void OnDocumentChanged(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string filenameText;
            if (document != null)
            {
                filenameText =
                    string.Format(ConstantStrings.ActiveFileFormat, _localizationService.Current.File, document.Name);
            }
            else
            {
                filenameText = _localizationService.Current.NoActiveFile;
            }
            
            PerformUpdate(filenameText);
        }
    }
}
