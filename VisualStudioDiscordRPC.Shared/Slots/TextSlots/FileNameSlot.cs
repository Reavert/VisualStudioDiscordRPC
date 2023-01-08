using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class FileNameSlot : TextSlot
    {
        private LocalizationService<LocalizationFile> _localizationService;
        private VsObserver _vsObserver;

        private Document _document;

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

            _document = document;
            Update();
        }

        protected override string GetData()
        {
            string data;
            if (_document != null)
            {
                data = string.Format(ConstantStrings.ActiveFileFormat, _localizationService.Current.File, _document.Name);
            }
            else
            {
                data = _localizationService.Current.NoActiveFile;
            }

            return data;
        }
    }
}
