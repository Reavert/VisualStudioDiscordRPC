﻿using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
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
        
        public void Init()
        {
            RegisterServices();
            UpdateSettings();

            _discordRpcController.Initialize();
            _slotService.InitSlotsSubscriptions();
            _vsObserver.Observe();
            _solutionHider.Start();

            ApplySettings();

            string currentExtensionVersion = VisualStudioHelper.GetExtensionVersion();
            bool updateNotificationsEnabled = bool.Parse(Settings.Default.UpdateNotifications);

            if (updateNotificationsEnabled && currentExtensionVersion != Settings.Default.Version)
            {
                Settings.Default.Version = currentExtensionVersion;
                Settings.Default.Save();

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
            var settingsService = new SettingsService();
            ServiceRepository.Default.AddService(settingsService);

            // Registering Visual Studio events observer.
            var currentDte = (DTE2)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            _vsObserver = new VsObserver(currentDte);

            ServiceRepository.Default.AddService(_vsObserver);

            // Registering localization service.
            var localizationService = new LocalizationService<LocalizationFile>(
                PathHelper.GetPackageInstallationPath(Settings.Default.TranslationsPath));
            localizationService.SelectLanguage(Settings.Default.Language);

            ServiceRepository.Default.AddService(localizationService);

            // Registering macro service.
            ServiceRepository.Default.AddService(new MacroService(_vsObserver));

            // Registering slot service.
            _slotService = new SlotService();
            ServiceRepository.Default.AddService(_slotService);

            // Registering Discord RPC controller.
            int updateTimeout = int.Parse(Settings.Default.UpdateTimeout);
            _discordRpcController = new DiscordRpcController(updateTimeout);
            ServiceRepository.Default.AddService(_discordRpcController);

            // Registering Solution Hider.
            _solutionHider = new SolutionHider(_vsObserver, _discordRpcController);
            ServiceRepository.Default.AddService(_solutionHider);
        }

        private void UpdateSettings()
        {
            if (!Settings.Default.Updated)
            {
                Settings.Default.Upgrade();
                Settings.Default.Updated = true;

                Settings.Default.Save();
            }
        }

        private void ApplySettings()
        {
            _discordRpcController.Enabled = bool.Parse(Settings.Default.RichPresenceEnabled);

            _discordRpcController.SetSlot<LargeIconUpdater>(_slotService.GetSlotByName<AssetSlot>(Settings.Default.LargeIconSlot));
            _discordRpcController.SetSlot<SmallIconUpdater>(_slotService.GetSlotByName<AssetSlot>(Settings.Default.SmallIconSlot));

            //_discordRpcController.SetSlot<DetailsUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.DetailsSlot));
            //_discordRpcController.SetSlot<StateUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.StateSlot));
            
            _discordRpcController.SetSlot<TimerUpdater>(_slotService.GetSlotByName<TimerSlot>(Settings.Default.TimerSlot));

            _discordRpcController.SetSlot<FirstButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(Settings.Default.FirstButtonSlot));
            _discordRpcController.SetSlot<SecondButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(Settings.Default.SecondButtonSlot));
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
