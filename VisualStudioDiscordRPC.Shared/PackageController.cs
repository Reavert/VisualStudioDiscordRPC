using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Services.Models;
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

            LoadSettings();
        }

        public void Clear()
        {
            _discordRpcController.Dispose();
            _slotService.ClearSlotsSubscriptions();
            _vsObserver.Unobserve();
        }

        private void RegisterServices()
        {
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
            _discordRpcController = new DiscordRpcController(1000);
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

        private void LoadSettings()
        {
            _discordRpcController.Enabled = bool.Parse(Settings.Default.RichPresenceEnabled);

            _discordRpcController.LargeIconSlot = _slotService.GetAssetSlotByName(Settings.Default.LargeIcon);
            _discordRpcController.SmallIconSlot = _slotService.GetAssetSlotByName(Settings.Default.SmallIcon);

            _discordRpcController.StateSlot = _slotService.GetTextSlotByName(Settings.Default.TitleText);
            _discordRpcController.DetailsSlot = _slotService.GetTextSlotByName(Settings.Default.SubTitleText);

            _discordRpcController.TimerSlot = _slotService.GetTimerSlotByName(Settings.Default.WorkTimerMode);
        }
    }
}
