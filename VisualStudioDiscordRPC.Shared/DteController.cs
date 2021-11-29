using DiscordRPC;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using VisualStudioDiscordRPC.Shared.Localization.Models;

namespace VisualStudioDiscordRPC.Shared
{
    internal class DteController : IDisposable
    {
        private readonly DTE _instance;
        private readonly DiscordRpcClient _client;
        private readonly RichPresence _presence;

        private readonly LocalizationManager<LocalizationFile> _localizationManager;

        public DteController()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Localization manager settings
            _localizationManager = new LocalizationManager<LocalizationFile>("Translations");
            _localizationManager.SelectLanguage("Russian");

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

            _presence = new RichPresence()
            {
                Details = _localizationManager.Current.NoActiveFile,
                State = _localizationManager.Current.NoActiveProject
            };

            _client.SetPresence(_presence);
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (GotFocus.Type == vsWindowType.vsWindowTypeDocument)
            {
                _presence.Details = $"{_localizationManager.Current.File}: {GotFocus.Caption}";
                _presence.State = $"{_localizationManager.Current.Project}: {GotFocus.Project.Name}";

                _client.SetPresence(_presence);
            }   
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance.Events.WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
            _client.Dispose();
        }
    }
}
