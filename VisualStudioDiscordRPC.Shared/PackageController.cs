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
        private readonly RichPresence _presence;

        public readonly LocalizationManager<LocalizationFile> LocalizationManager;

        private readonly IAssetMap<ExtensionAsset> _extensionsAssetMap;
        private readonly ExtensionAssetComparer _extensionAssetComparer;

        private readonly string _installationPath;

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

            _extensionAssetComparer = new ExtensionAssetComparer();

            // Localization manager settings
            LocalizationManager = new LocalizationManager<LocalizationFile>(GetLocalFilePath("Translations"));
            LocalizationManager.LocalizationChanged += LocalizationManager_LocalizationChanged;

            // Discord Rich Presense client settings
            _client = new DiscordRpcClient("914622396630175855");
            _client.Initialize();

            _presence = new RichPresence()
            {
                Assets = new Assets()
            };

            LocalizationManager.SelectLanguage(Settings.Default.Language);
        }

        private void LocalizationManager_LocalizationChanged()
        {
            UpdateText(true);
        }

        public void UpdateText(bool update)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            Document activeDocument = _instance.ActiveDocument;

            if (activeDocument != null)
            {
                _presence.Details = $"{LocalizationManager.Current.File} {activeDocument.Name}";
                _presence.State = $"{LocalizationManager.Current.Project} {activeDocument.ActiveWindow.Project.Name}";
                _presence.Timestamps = Timestamps.Now;
            }
            else
            {
                _presence.Details = LocalizationManager.Current.NoActiveFile;
                _presence.State = LocalizationManager.Current.NoActiveProject;
                _presence.Timestamps = null;
            }

            if (update)
            {
                _client.SetPresence(_presence);
            }
        }

        public void UpdateIcon(bool update)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (_instance.ActiveDocument != null)
            {
                string fileExtension = Path.GetExtension(_instance.ActiveDocument.Name);

                _extensionAssetComparer.RequiredExtension = fileExtension;

                ExtensionAsset extensionAsset =
                        _extensionsAssetMap.GetAsset(_extensionAssetComparer) ?? ExtensionAsset.Default;

                _presence.Assets.LargeImageKey = extensionAsset.Key;
                _presence.Assets.LargeImageText = extensionAsset.Name;
            }
            else
            {
                _presence.Assets.LargeImageKey = string.Empty;
                _presence.Assets.LargeImageText = string.Empty;
            }
            
            if (update)
            {
                _client.SetPresence(_presence);
            }
        }

        public void UpdateRichPresence()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            UpdateText(false);
            UpdateIcon(false);

            _client.SetPresence(_presence);
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();
            
            UpdateRichPresence();
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
