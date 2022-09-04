using DiscordRPC;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE80;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Slots;
using EnvDTE;


namespace VisualStudioDiscordRPC.Shared
{
    public class PackageController : IDisposable
    {
        private readonly DiscordRpcClient _client;
        private readonly string _installationPath;

        private readonly LocalizationService<LocalizationFile> _localizationService;
        public RichPresenceWrapper RichPresenceWrapper;

        private IObserver _observer;

        private string GetLocalFilePath(string filename)
        {
            return Path.Combine(_installationPath, filename);
        }
       
        public PackageController(DTE2 instance, string installationPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            ServiceProvider serviceProvider = ServiceProvider.GlobalProvider;
            var currentDte = (DTE2)serviceProvider.GetService(typeof(DTE));
            if (currentDte == null)
            {
                throw new InvalidOperationException("Can not get DTE Service");
            }

            _observer = new VsObserver(currentDte);
            _installationPath = installationPath;

            // Extension asset map settings
            IAssetMap<ExtensionAsset> extensionsAssetMap = new OptimizedAssetMap<ExtensionAsset>();

            var extensionAssetLoader = new JsonAssetsLoader<ExtensionAsset>();
            extensionsAssetMap.Assets = new List<ExtensionAsset>(extensionAssetLoader.LoadAssets(GetLocalFilePath("extensions_assets_map.json")));

            // Discord Rich Presence client settings
            _client = new DiscordRpcClient(Settings.Default.ApplicationID);
            _client.Initialize();

            // Localization service settings
            _localizationService = new LocalizationService<LocalizationFile>(GetLocalFilePath(Settings.Default.TranslationsPath));
            ServiceRepository.Default.AddService(_localizationService);
            _localizationService.SelectLanguage(Settings.Default.Language);

            if (!Settings.Default.Updated)
            {
                Settings.Default.Upgrade();
                Settings.Default.Updated = true;

                Settings.Default.Save();
            }

            var largeIconUpdater = new SlotUpdateHandler((string data) => _client.UpdateLargeAsset(data));
            var extensionIconSlot = new ExtensionIconSlot(extensionsAssetMap, _observer);
            largeIconUpdater.Slot = extensionIconSlot;

            var detailsTextUpdater = new SlotUpdateHandler((string data) => _client.UpdateDetails(data));
            var filenameSlot = new FileNameSlot(_observer);
            detailsTextUpdater.Slot = filenameSlot;

            var stateTextUpdater = new SlotUpdateHandler((string data) => _client.UpdateState(data));
            var projectNameSlot = new ProjectNameSlot(_observer);
            stateTextUpdater.Slot = projectNameSlot;

            _observer.Observe();
            extensionIconSlot.Enable();
            filenameSlot.Enable();
            projectNameSlot.Enable();

            /*// RP Wrapper settings
            RichPresenceWrapper = new RichPresenceWrapper(_client)
            {
                Dte = _instance,
                ExtensionAssets = extensionsAssetMap,

                LargeIcon = Settings.Default.LargeIcon == null 
                    ? RichPresenceWrapper.Icon.FileExtension
                    : SettingsHelper.Instance.IconEnumMap.GetEnumValue(Settings.Default.LargeIcon),
                SmallIcon = Settings.Default.SmallIcon == null
                    ? RichPresenceWrapper.Icon.VisualStudioVersion
                    : SettingsHelper.Instance.IconEnumMap.GetEnumValue(Settings.Default.SmallIcon),
                TitleText = Settings.Default.TitleText == null
                    ? RichPresenceWrapper.Text.FileName
                    : SettingsHelper.Instance.TextEnumMap.GetEnumValue(Settings.Default.TitleText),
                SubTitleText = Settings.Default.SubTitleText == null
                    ? RichPresenceWrapper.Text.SolutionName
                    : SettingsHelper.Instance.TextEnumMap.GetEnumValue(Settings.Default.SubTitleText),
                WorkTimerMode = Settings.Default.WorkTimerMode == null
                    ? RichPresenceWrapper.TimerMode.File
                    : SettingsHelper.Instance.TimerModeEnumMap.GetEnumValue(Settings.Default.WorkTimerMode),
                GitLinkVisible = Settings.Default.GitLinkVisible != null && bool.Parse(Settings.Default.GitLinkVisible)
            };*/
        }

        public void Dispose()
        {
            /*ThreadHelper.ThrowIfNotOnUIThread();

            _localizationService.LocalizationChanged -= OnLocalizationChanged;
            
            _client.Dispose();*/
        }
    }
}
