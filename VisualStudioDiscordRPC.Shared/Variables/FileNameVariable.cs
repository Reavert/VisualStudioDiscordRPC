using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class FileNameVariable : Variable
    {
        private Document _document;

        public FileNameVariable(VsObserver vsObserver) 
        {
            _document = vsObserver.DTE.ActiveDocument;
            vsObserver.DocumentChanged += OnDocumentChanged;
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
