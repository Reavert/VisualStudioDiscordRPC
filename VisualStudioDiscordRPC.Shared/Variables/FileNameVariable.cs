using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class FileNameVariable : Variable
    {
        private Document _document;
        private VsObserver _vsObserver;

        public FileNameVariable(VsObserver vsObserver) 
        {
            _vsObserver = vsObserver;
        }

        public override void Initialize()
        {
            _document = _vsObserver.DTE.ActiveDocument;
            _vsObserver.DocumentChanged += OnDocumentChanged;
        }

        public override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return _document?.Name;
        }

        private void OnDocumentChanged(Document document)
        {
            _document = document;
            RaiseChangedEvent();
        }
    }
}
