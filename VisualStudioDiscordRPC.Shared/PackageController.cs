using DiscordRPC;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
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
        public readonly LocalizationManager<LocalizationFile> LocalizationManager;
        private readonly IAssetMap<ExtensionAsset> _extensionsAssetMap;
        private readonly string _installationPath;
        private RichPresenceWrapper _wrapper;

        private readonly Dictionary<int, int> _versions = new Dictionary<int, int>()
        {
            { 16, 2019 },
            { 17, 2022 }
        };

        private int GetVersionMajor(string version)
        {
            return int.Parse(version.Split('.')[0]);
        }

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
            _wrapper = new RichPresenceWrapper(_client)
            {
                DTE = _instance,
                ExtensionAssets = _extensionsAssetMap
            };

            // Localization manager settings
            LocalizationManager = new LocalizationManager<LocalizationFile>(GetLocalFilePath(Settings.Default.TranslationsPath));
            LocalizationManager.LocalizationChanged += LocalizationManager_LocalizationChanged;
            LocalizationManager.SelectLanguage(Settings.Default.Language);
        }

        private void LocalizationManager_LocalizationChanged()
        {
            _wrapper.Localization = LocalizationManager.Current;
            _wrapper.Update();
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var activeDocument = _instance.ActiveDocument;

            if (activeDocument == null)
            {
                _wrapper.LargeIcon = RichPresenceWrapper.Icon.VisualStudioVersion;
                _wrapper.SmallIcon = RichPresenceWrapper.Icon.None;
            }
            else
            {
                _wrapper.LargeIcon = RichPresenceWrapper.Icon.FileExtension;
                _wrapper.SmallIcon = RichPresenceWrapper.Icon.VisualStudioVersion;
                _wrapper.Document = _instance.ActiveDocument;
            }
            
            _wrapper.Update();
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
