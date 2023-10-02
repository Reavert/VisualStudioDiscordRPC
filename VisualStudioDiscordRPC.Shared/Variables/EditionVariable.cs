using EnvDTE;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class EditionVariable : Variable
    {
        private readonly string _edition;

        public EditionVariable(DTE dte)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            _edition = dte.Edition;
        }

        public override string GetData()
        {
            return _edition;
        }
    }
}
