using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class FileNameSlot : TextSlot
    {
        private readonly LocalizationService<LocalizationFile> _localizationService;
        private readonly VsObserver _vsObserver;

        private Document _document;

        public FileNameSlot(VsObserver vsObserver)
        {
            _localizationService = ServiceRepository.Default.GetService<LocalizationService<LocalizationFile>>();
            _vsObserver = vsObserver;
            _document = _vsObserver.DTE.ActiveDocument;
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
            _document = document;

            Update();
        }

        protected override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (_document == null)
            {
                return _localizationService.Current.NoActiveFile;
            }

            return string.Format(ConstantStrings.ActiveFileFormat, _localizationService.Current.File, _document.Name);
        }
    }
}
