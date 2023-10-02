using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.ReleaseNotes;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs;
using VisualStudioDiscordRPC.Shared.Nests;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared
{
    public class PackageController
    {
        private SettingsService _settingsService;
        private DiscordRpcController _discordRpcController;
        private VsObserver _vsObserver;
        private GitObserver _gitObserver;
        private PlugService _plugService;
        private SolutionSecrecyService _solutionPrivateService;
        private RepositorySecrecyService _repositorySecrecyService;

        public void Init()
        {
            RegisterServices();

            _discordRpcController.Initialize();
            _plugService.InitPlugsSubscriptions();
            _vsObserver.Observe();
            _gitObserver.Observe();
            _solutionPrivateService.Start();
            _repositorySecrecyService.Start();

            ApplySettings();

            string currentExtensionVersion = VisualStudioHelper.GetExtensionVersion();
            bool updateNotificationsEnabled = _settingsService.Read<bool>(SettingsKeys.UpdateNotifications);

            string previousVersion = _settingsService.Read<string>(SettingsKeys.Version);

            if (updateNotificationsEnabled && currentExtensionVersion != previousVersion)
            {
                _settingsService.Set(SettingsKeys.Version, currentExtensionVersion);
                _settingsService.Save();

                DisplayVersionUpdateMessage(currentExtensionVersion);
            }
        }

        public void Clear()
        {
            _discordRpcController.Dispose();
            _plugService.ClearPlugsSubscriptions();
            _vsObserver.Unobserve();
            _gitObserver.Unobserve();
            _solutionPrivateService.Stop();
            _repositorySecrecyService.Stop();
        }

        private void RegisterServices()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Registering settings service.
            _settingsService = new SettingsService();
            ServiceRepository.Default.AddService(_settingsService);

            // Registering Visual Studio events observer.
            var currentDte = (DTE2)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            _vsObserver = new VsObserver(currentDte);

            ServiceRepository.Default.AddService(_vsObserver);

            // Registering Git observer.
            _gitObserver = new GitObserver(_vsObserver);
            ServiceRepository.Default.AddService(_gitObserver);

            // Registering localization service
            string translationPath = _settingsService.Read<string>(SettingsKeys.TranslationsPath);
            string currentLanguage = _settingsService.Read<string>(SettingsKeys.Language);

            var localizationService = new LocalizationService(PathHelper.GetPackageInstallationPath(translationPath));
            localizationService.SelectLanguage(currentLanguage);

            ServiceRepository.Default.AddService(localizationService);

            // Registering variable service.
            ServiceRepository.Default.AddService(new VariableService(_vsObserver));

            // Registering plug service.
            _plugService = new PlugService();
            ServiceRepository.Default.AddService(_plugService);

            // Registering Discord RPC controller.
            int updateTimeout = Convert.ToInt32(_settingsService.Read<long>(SettingsKeys.UpdateTimeout));
            _discordRpcController = new DiscordRpcController(updateTimeout);
            ServiceRepository.Default.AddService(_discordRpcController);

            // Registering Solution Secrecy Service.
            _solutionPrivateService = new SolutionSecrecyService(_vsObserver, _discordRpcController);
            ServiceRepository.Default.AddService(_solutionPrivateService);

            // Registering Repository Secrecy Service.
            var gitRepositoryButtons = _plugService.GetPlugsOfType<GitRepositoryButtonPlug>().ToArray();
            _repositorySecrecyService = new RepositorySecrecyService(_gitObserver, gitRepositoryButtons);
            ServiceRepository.Default.AddService(_repositorySecrecyService);
        }
        
        private void ApplySettings()
        {
            _discordRpcController.Enabled = _settingsService.Read<bool>(SettingsKeys.RichPresenceEnabled);

            string largeIconPlug = _settingsService.Read<string>(SettingsKeys.LargeIconPlug);
            _discordRpcController.SetPlug<LargeIconNest>(_plugService.GetPlugById<BaseAssetPlug>(largeIconPlug));

            string smallIconPlug = _settingsService.Read<string>(SettingsKeys.SmallIconPlug);
            _discordRpcController.SetPlug<SmallIconNest>(_plugService.GetPlugById<BaseAssetPlug>(smallIconPlug));

            string detailsPlug = _settingsService.Read<string>(SettingsKeys.DetailsPlug);
            _discordRpcController.SetPlug<DetailsNest>(_plugService.GetPlugById<BaseTextPlug>(detailsPlug));

            string statePlug = _settingsService.Read<string>(SettingsKeys.StatePlug);
            _discordRpcController.SetPlug<StateNest>(_plugService.GetPlugById<BaseTextPlug>(statePlug));

            string timerPlug = _settingsService.Read<string>(SettingsKeys.TimerPlug);
            _discordRpcController.SetPlug<TimerNest>(_plugService.GetPlugById<BaseTimerPlug>(timerPlug));

            string firstButtonPlug = _settingsService.Read<string>(SettingsKeys.FirstButtonPlug);
            _discordRpcController.SetPlug<FirstButtonNest>(_plugService.GetPlugById<BaseButtonPlug>(firstButtonPlug));

            string secondButtonPlug = _settingsService.Read<string>(SettingsKeys.SecondButtonPlug);
            _discordRpcController.SetPlug<SecondButtonNest>(_plugService.GetPlugById<BaseButtonPlug>(secondButtonPlug));
        }

        private void DisplayVersionUpdateMessage(string version)
        {
            var updateTextBuilder = new StringBuilder();
            updateTextBuilder.AppendLine(string.Format(ConstantStrings.NewVersionNotification, version));

            ReleaseNote currentVersionReleaseNote = ReadReleaseNoteOfVersion(version);
            if (currentVersionReleaseNote != null)
            {
                updateTextBuilder.AppendLine();
                updateTextBuilder.AppendLine("Release notes: ");

                string notesText = string.Join("\n", 
                    currentVersionReleaseNote.Notes.Select(note => $" - {note}"));
                updateTextBuilder.AppendLine(notesText);
            }

            MessageBox.Show(updateTextBuilder.ToString(), "Visual Studio Discord RPC Update",
                    MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private ReleaseNote ReadReleaseNoteOfVersion(string version)
        {
            string releaseNotePath = PathHelper.GetPackageInstallationPath("RELEASE_NOTES.txt");
            string releaseNotesText = File.ReadAllText(releaseNotePath);

            var releaseNotesParser = new ReleaseNotesParser(releaseNotesText);

            while (releaseNotesParser.ReadReleaseNote(out ReleaseNote releaseNote))
            {
                if (releaseNote.Version == version)
                {
                    return releaseNote;
                }
            }

            return null;
        }
    }
}
