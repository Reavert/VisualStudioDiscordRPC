using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.ReleaseNotes;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Updaters;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared
{
    public class PackageController
    {
        private DiscordRpcController _discordRpcController;
        private VsObserver _vsObserver;
        private SlotService _slotService;
        private SolutionHider _solutionHider;
        private SettingsService _settingsService;

        public void Init()
        {
            RegisterServices();

            _discordRpcController.Initialize();
            _slotService.InitSlotsSubscriptions();
            _vsObserver.Observe();
            _solutionHider.Start();

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
            _slotService.ClearSlotsSubscriptions();
            _vsObserver.Unobserve();
            _solutionHider.Stop();
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

            // Registering localization service.

            string translationPath = _settingsService.Read<string>(SettingsKeys.TranslationsPath);
            string currentLanguage = _settingsService.Read<string>(SettingsKeys.Language);

            var localizationService = new LocalizationService<LocalizationFile>(PathHelper.GetPackageInstallationPath(translationPath));
            localizationService.SelectLanguage(currentLanguage);

            ServiceRepository.Default.AddService(localizationService);

            // Registering macro service.
            ServiceRepository.Default.AddService(new MacroService(_vsObserver));

            // Registering slot service.
            _slotService = new SlotService();
            ServiceRepository.Default.AddService(_slotService);

            // Registering Discord RPC controller.
            int updateTimeout = Convert.ToInt32(_settingsService.Read<long>(SettingsKeys.UpdateTimeout));
            _discordRpcController = new DiscordRpcController(updateTimeout);
            ServiceRepository.Default.AddService(_discordRpcController);

            // Registering Solution Hider.
            _solutionHider = new SolutionHider(_vsObserver, _discordRpcController);
            ServiceRepository.Default.AddService(_solutionHider);
        }
        
        private void ApplySettings()
        {
            _discordRpcController.Enabled = _settingsService.Read<bool>(SettingsKeys.RichPresenceEnabled);

            string largeIconSlot = _settingsService.Read<string>(SettingsKeys.LargeIconSlot);
            _discordRpcController.SetSlot<LargeIconUpdater>(_slotService.GetSlotByName<AssetSlot>(largeIconSlot));

            string smallIconSlot = _settingsService.Read<string>(SettingsKeys.SmallIconSlot);
            _discordRpcController.SetSlot<SmallIconUpdater>(_slotService.GetSlotByName<AssetSlot>(smallIconSlot));

            //_discordRpcController.SetSlot<DetailsUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.DetailsSlot));
            //_discordRpcController.SetSlot<StateUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.StateSlot));

            string timerSlot = _settingsService.Read<string>(SettingsKeys.TimerSlot);
            _discordRpcController.SetSlot<TimerUpdater>(_slotService.GetSlotByName<TimerSlot>(timerSlot));

            string firstButtonSlot = _settingsService.Read<string>(SettingsKeys.FirstButtonSlot);
            _discordRpcController.SetSlot<FirstButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(firstButtonSlot));

            string secondButtonSlot = _settingsService.Read<string>(SettingsKeys.SecondButtonSlot);
            _discordRpcController.SetSlot<SecondButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(secondButtonSlot));
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
