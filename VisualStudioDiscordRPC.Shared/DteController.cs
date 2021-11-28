using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace VisualStudioDiscordRPC
{
    internal class DteController : IDisposable
    {
        DTE _instance;

        public DteController()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));

            if (_instance == null)
            {
                throw new InvalidOperationException("Can not get DTE Service");
            }

            _instance.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            System.Windows.MessageBox.Show($"Caption: {GotFocus.Caption}\nType: {GotFocus?.Type}");
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance.Events.WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
        }
    }
}
