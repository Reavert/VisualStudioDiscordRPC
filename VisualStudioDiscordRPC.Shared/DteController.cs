using DiscordRPC;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Localization.Interfaces;
using VisualStudioDiscordRPC.Shared.Localization.Models;

namespace VisualStudioDiscordRPC.Shared
{
    internal class DteController : IDisposable
    {
        private readonly DTE _instance;
        private readonly DiscordRpcClient _client;
        private readonly RichPresence _presence;

        private readonly ILocalizationManager<LocalizationFile> _localizationManager;

        private readonly IAssetMap<ExtensionAsset> _extensionsAssetMap;
        private readonly ExtensionAssetComparer _extensionAssetComparer;

        private readonly string _installationPath;

        private string GetLocalFilePath(string filename)
        {
            return Path.Combine(_installationPath, filename);
        }

        public DteController(string installationPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _installationPath = installationPath;

            // Extension asset map settings
            _extensionsAssetMap = new AssetMap<ExtensionAsset>();

            var extensionAssetLoader = new JsonAssetsLoader<ExtensionAsset>();
            _extensionsAssetMap.Assets = extensionAssetLoader.LoadAssets(GetLocalFilePath("extensions_assets_map.json"));

            _extensionAssetComparer = new ExtensionAssetComparer();

            // Localization manager settings
            _localizationManager = new LocalizationManager<LocalizationFile>(GetLocalFilePath("Translations"));
            _localizationManager.SelectLanguage("English");

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
                State = _localizationManager.Current.NoActiveProject,
                Assets = new Assets()
            };

            _client.SetPresence(_presence);
        }

        private void WindowEvents_WindowActivated(Window GotFocus, Window LostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (GotFocus.Type == vsWindowType.vsWindowTypeDocument)
            {
                _presence.Details = $"{_localizationManager.Current.File} {GotFocus.Caption}";
                _presence.State = $"{_localizationManager.Current.Project} {GotFocus.Project.Name}";
                _presence.Timestamps = Timestamps.Now;

                string fileExtension = Path.GetExtension(GotFocus.Caption);
                
                _extensionAssetComparer.RequiredExtension = fileExtension;
                ExtensionAsset extensionAsset = 
                    _extensionsAssetMap.GetAsset(_extensionAssetComparer) ?? ExtensionAsset.Default;
                
                _presence.Assets = new Assets()
                {
                    LargeImageKey = extensionAsset.Key,
                    LargeImageText = extensionAsset.Name
                };

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
