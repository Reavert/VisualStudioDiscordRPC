using EnvDTE80;

namespace VisualStudioDiscordRPC.Shared.Variables
{
    public class EditionVariable : Variable
    {
        private readonly DTE2 _dte;
        private string _edition;

        public EditionVariable(DTE2 dte)
        {
            _dte = dte;
        }

        public override void Initialize()
        {
            _edition = _dte.Edition;
        }

        public override string GetData()
        {
            return _edition;
        }
    }
}
