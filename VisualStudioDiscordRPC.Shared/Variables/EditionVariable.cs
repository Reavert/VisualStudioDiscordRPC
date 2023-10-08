using EnvDTE80;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class EditionVariable : Variable
    {
        private readonly string _edition;

        public EditionVariable(DTE2 dte)
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
