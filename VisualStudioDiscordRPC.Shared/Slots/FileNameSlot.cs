using EnvDTE;
using System;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class FileNameSlot : AbstractSlot
    {
        private LocalizationService<LocalizationFile> _localizationService;

        public FileNameSlot(IObserver observer) : base(observer)
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
        }

        public override void Enable()
        {
            Observer.DocumentChanged += OnDocumentChanged;
        }

        public override void Disable()
        {
            Observer.DocumentChanged -= OnDocumentChanged;
        }

        private void OnDocumentChanged(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string filenameText = string.Empty;
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
