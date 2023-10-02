using EnvDTE;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public class FileScopeTimerPlug : BaseTimerPlug
    {
        private readonly VsObserver _vsObserver;

        public FileScopeTimerPlug(VsObserver vsObserver) : base()
        {
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
            SyncTimestamp();
        }
    }
}
