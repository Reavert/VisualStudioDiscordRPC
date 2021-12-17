using DiscordRPC;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Localization.Models;

namespace VisualStudioDiscordRPC.Shared
{
    public class PackageController : IDisposable
    {
        private readonly DTE _instance;
        private readonly DiscordRpcClient _client;
        private readonly IAssetMap<ExtensionAsset> _extensionsAssetMap;
        private readonly string _installationPath;

        public readonly LocalizationManager<LocalizationFile> LocalizationManager;
        public RichPresenceWrapper RichPresenceWrapper;

        private string GetLocalFilePath(string filename)
        {
            return Path.Combine(_installationPath, filename);
        }

        public PackageController(DTE instance, string installationPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance = instance;
            _instance.Events.WindowEvents.WindowActivated += WindowEvents_WindowActivated;

            _installationPath = installationPath;

            // Extension asset map settings
            _extensionsAssetMap = new AssetMap<ExtensionAsset>();

            var extensionAssetLoader = new JsonAssetsLoader<ExtensionAsset>();
            _extensionsAssetMap.Assets = extensionAssetLoader.LoadAssets(GetLocalFilePath("extensions_assets_map.json"));

            // Discord Rich Presense client settings
            _client = new DiscordRpcClient(Settings.Default.ApplicationID);
            _client.Initialize();

            // RP Wrapper settings
            RichPresenceWrapper = new RichPresenceWrapper(_client)
            {
                Dte = _instance,
                ExtensionAssets = _extensionsAssetMap
            };

            // Localization manager settings
            LocalizationManager = new LocalizationManager<LocalizationFile>(GetLocalFilePath(Settings.Default.TranslationsPath));
            LocalizationManager.LocalizationChanged += LocalizationManager_LocalizationChanged;
            LocalizationManager.SelectLanguage(Settings.Default.Language);
        }

        private void LocalizationManager_LocalizationChanged()
        {
            RichPresenceWrapper.Localization = LocalizationManager.Current;
            RichPresenceWrapper.Update();
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            RichPresenceWrapper.Document = _instance.ActiveDocument;
            RichPresenceWrapper.Update();
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance.Events.WindowEvents.WindowActivated -= WindowEvents_WindowActivated;
            LocalizationManager.LocalizationChanged -= LocalizationManager_LocalizationChanged;

            _client.Dispose();
        }
    }
}
