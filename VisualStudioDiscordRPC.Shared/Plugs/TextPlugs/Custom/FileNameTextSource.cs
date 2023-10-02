using EnvDTE;
using System;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public class FileNameTextSource : ITextSource
    {
        public string Name => "filename";

        public string Text => _fileName;

        public event Action Changed;

        private VsObserver _vsObserver;
        private string _fileName;

        public FileNameTextSource(VsObserver vsObserver)
        {
            _vsObserver = vsObserver;
            _vsObserver.DocumentChanged += OnDocumentChanged;
            _fileName = _vsObserver.DTE.ActiveDocument?.Name;
        }

        private void OnDocumentChanged(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            
            if (document == null)
            {
                return;
            }

            _fileName = document.Name;

            Changed?.Invoke();
        }
    }
}
