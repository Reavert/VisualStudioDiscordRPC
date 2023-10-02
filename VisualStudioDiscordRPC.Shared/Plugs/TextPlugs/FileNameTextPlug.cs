using EnvDTE;
using VisualStudioDiscordRPC.Shared.Services;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class FileNameTextPlug : BaseTextPlug
    {
        private readonly VsObserver _vsObserver;
        private readonly LocalizationService _localizationService;

        private Document _document;

        public FileNameTextPlug(VsObserver vsObserver, LocalizationService localizationService)
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
                return _localizationService.Localize(LocalizationKeys.NoActiveFile);
            }

            return string.Format("{0} {1}", _localizationService.Localize(LocalizationKeys.File), _document.Name);
        }
    }
}
