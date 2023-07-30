using EnvDTE;

namespace VisualStudioDiscordRPC.Shared.Macros
{
    public class EditionMacro : Macro
    {
        private readonly string _edition;

        public EditionMacro(DTE dte)
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
