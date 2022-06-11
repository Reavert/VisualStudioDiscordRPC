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
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Localization.Models;
using VisualStudioDiscordRPC.Shared.Services.Models;

namespace VisualStudioDiscordRPC.Shared
{
    public class PackageController : IDisposable
    {
        private readonly DTE _instance;
        private readonly DiscordRpcClient _client;
        private readonly string _installationPath;

        private readonly LocalizationService<LocalizationFile> _localizationService;
        public RichPresenceWrapper RichPresenceWrapper;

        private string GetLocalFilePath(string filename)
        {
            return Path.Combine(_installationPath, filename);
        }

        public PackageController(DTE instance, string installationPath)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance = instance;
            _instance.Events.WindowEvents.WindowActivated += OnWindowActivated;

            _installationPath = installationPath;

            // Extension asset map settings
            IAssetMap<ExtensionAsset> extensionsAssetMap = new OptimizedAssetMap<ExtensionAsset>();

            var extensionAssetLoader = new JsonAssetsLoader<ExtensionAsset>();
            extensionsAssetMap.Assets = new List<ExtensionAsset>(extensionAssetLoader.LoadAssets(GetLocalFilePath("extensions_assets_map.json")));

            // Discord Rich Presence client settings
            _client = new DiscordRpcClient(Settings.Default.ApplicationID);
            _client.Initialize();

            // RP Wrapper settings

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
            };

            // Localization service settings
            _localizationService = new LocalizationService<LocalizationFile>(GetLocalFilePath(Settings.Default.TranslationsPath));
            ServiceRepository.Default.AddService(_localizationService);
            
            _localizationService.LocalizationChanged += OnLocalizationChanged;
            _localizationService.SelectLanguage(Settings.Default.Language);
        }

        private void OnLocalizationChanged()
        {
            RichPresenceWrapper.Localization = _localizationService.Current;
            RichPresenceWrapper.Update();
        }

        private void OnWindowActivated(Window gotFocus, Window lostFocus)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            RichPresenceWrapper.Document = _instance.ActiveDocument;
            RichPresenceWrapper.Update();
        }

        public void Dispose()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            _instance.Events.WindowEvents.WindowActivated -= OnWindowActivated;
            _localizationService.LocalizationChanged -= OnLocalizationChanged;

            _client.Dispose();
        }
    }
}
