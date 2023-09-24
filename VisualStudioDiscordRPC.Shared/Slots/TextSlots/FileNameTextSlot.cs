using EnvDTE;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class FileNameTextSlot : TextSlot
    {
        private readonly VsObserver _vsObserver;
        private readonly LocalizationService<LocalizationFile> _localizationService;

        private Document _document;

        public FileNameTextSlot(VsObserver vsObserver, LocalizationService<LocalizationFile> localizationService)
        {
            _vsObserver = vsObserver;
            _localizationService = localizationService;

            _document = vsObserver.DTE.ActiveDocument;
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

            return string.Format("{0} {1}", _localizationService.Current.File, _document.Name);
        }
    }
}
