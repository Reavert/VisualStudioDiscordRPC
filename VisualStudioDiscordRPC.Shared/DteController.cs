using DiscordRPC;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;

namespace VisualStudioDiscordRPC
{
    internal class DteController : IDisposable
    {
        DTE _instance;
        DiscordRpcClient _client;

        public DteController()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // DTE settings
            _instance = (DTE)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            
            if (_instance == null)
            {
                throw new InvalidOperationException("Can not get DTE Service");
            }

            _instance.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;

            // Discord Rich Presense client settings
            _client = new DiscordRpcClient("914622396630175855");

            _client.Initialize();

            _client.SetPresence(new RichPresence()
            {
                Details = "Example Project",
                State = "csharp example",
                Assets = new Assets()
                {
                    LargeImageKey = "image_large",
                    LargeImageText = "Lachee's Discord IPC Library",
                    SmallImageKey = "image_small"
                }
            });
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
            _client.Dispose();
        }
    }
}
