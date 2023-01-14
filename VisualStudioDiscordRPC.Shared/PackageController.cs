using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.Windows;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
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
        
        public void Init()
        {
            RegisterServices();
            UpdateSettings();

            _discordRpcController.Initialize();
            _slotService.InitSlotsSubscriptions();
            _vsObserver.Observe();

            ApplySettings();

            string currentExtensionVersion = VisualStudioHelper.GetExtensionVersion();
            bool updateNotificationsEnabled = bool.Parse(Settings.Default.UpdateNotifications);

            if (updateNotificationsEnabled && currentExtensionVersion != Settings.Default.Version)
            {
                Settings.Default.Version = currentExtensionVersion;
                Settings.Default.Save();

                MessageBox.Show(string.Format(ConstantStrings.NewVersionNotification, currentExtensionVersion),
                    "Update", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Clear()
        {
            _discordRpcController.Dispose();
            _slotService.ClearSlotsSubscriptions();
            _vsObserver.Unobserve();
        }

        private void RegisterServices()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Registering Visual Studio events observer.
            var currentDte = (DTE2)ServiceProvider.GlobalProvider.GetService(typeof(DTE));
            _vsObserver = new VsObserver(currentDte);

            ServiceRepository.Default.AddService(_vsObserver);

            // Registering localization service.
            var localizationService = new LocalizationService<LocalizationFile>(
                PackageFileHelper.GetPackageFilePath(Settings.Default.TranslationsPath));
            localizationService.SelectLanguage(Settings.Default.Language);

            ServiceRepository.Default.AddService(localizationService);

            // Registering slot service.
            _slotService = new SlotService();
            ServiceRepository.Default.AddService(_slotService);

            // Registering Discord RPC controller.
            int updateTimeout = int.Parse(Settings.Default.UpdateTimeout);
            _discordRpcController = new DiscordRpcController(updateTimeout);
            ServiceRepository.Default.AddService(_discordRpcController);
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

            _discordRpcController.SetSlot<DetailsUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.DetailsSlot));
            _discordRpcController.SetSlot<StateUpdater>(_slotService.GetSlotByName<TextSlot>(Settings.Default.StateSlot));
            
            _discordRpcController.SetSlot<TimerUpdater>(_slotService.GetSlotByName<TimerSlot>(Settings.Default.TimerSlot));

            _discordRpcController.SetSlot<FirstButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(Settings.Default.FirstButtonSlot));
            _discordRpcController.SetSlot<SecondButtonUpdater>(_slotService.GetSlotByName<ButtonSlot>(Settings.Default.SecondButtonSlot));
        }
    }
}
