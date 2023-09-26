using EnvDTE;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots
{
    public class VisualStudioVersionTextSlot : TextSlot
    {
        private readonly DTE _dte;

        public VisualStudioVersionTextSlot(DTE dte) 
        {
            _dte = dte;
        }

        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override string GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            return string.Format("Visual Studio {0} {1}", _dte.Edition, VisualStudioHelper.GetVersionByDevNumber(_dte.Version));
        }
    }
}
