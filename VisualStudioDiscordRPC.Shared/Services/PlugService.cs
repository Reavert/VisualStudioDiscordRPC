using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Localization;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Plugs;
using VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.ButtonPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TextPlugs;
using VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class PlugService
    {
        private const string ExtensionAssetMapFilename = "extensions_assets_map.json";
        private const string VsVersionAssetMapFilename = "vs_assets_map.json";

        private readonly List<BasePlug> _plugs = new List<BasePlug>();

        private readonly VsObserver _vsObserver = ServiceRepository.Default.GetService<VsObserver>();
        private readonly VariableService _variableService = ServiceRepository.Default.GetService<VariableService>();

        public PlugService()
        {
            LoadPlugs();
        }

        public void LoadPlugs()
        {
            LoadAssetPlugs();
            LoadTextPlugs();
            LoadTimerPlugs();
            LoadButtonPlugs();
        }

        public void InitPlugsSubscriptions()
        {
            foreach (BasePlug plug in _plugs)
            {
                plug.Enable();
            }
        }

        public void ClearPlugsSubscriptions()
        {
            foreach (BasePlug plug in _plugs)
            {
                plug.Disable();
            }
        }

        public TPlug GetPlugById<TPlug>(string id) where TPlug : BasePlug
        {
            return (TPlug) _plugs.FirstOrDefault(plug => plug.GetId() == id);
        }

        public IReadOnlyList<TPlug> GetPlugsOfType<TPlug>() where TPlug : BasePlug
        {
            return _plugs
                .Where(plug => plug is TPlug)
                .Select(plug => (TPlug) plug)
                .ToList();
        }

        public IReadOnlyList<CustomTextPlugData> GetCustomTextPlugsData()
        {
            var appDataPath = PathHelper.GetApplicationDataPath();
            var customPlugsFilePath = Path.Combine(appDataPath, "custom_plugs.json");
            if (!File.Exists(customPlugsFilePath))
                return (IReadOnlyList<CustomTextPlugData>)Enumerable.Empty<CustomTextPlugData>();

            string data = File.ReadAllText(customPlugsFilePath);
            if (string.IsNullOrEmpty(data))
                return (IReadOnlyList<CustomTextPlugData>)Enumerable.Empty<CustomTextPlugData>();

            var customTextPlugsData = JsonConvert.DeserializeObject<List<CustomTextPlugData>>(data);
            if (customTextPlugsData == null)
                return (IReadOnlyList<CustomTextPlugData>)Enumerable.Empty<CustomTextPlugData>();

            return customTextPlugsData;
        }

        public void SaveCustomTextPlugsData(IEnumerable<CustomTextPlugData> data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            var appDataPath = PathHelper.GetApplicationDataPath();
            var customPlugsFilePath = Path.Combine(appDataPath, "custom_plugs.json");
            File.WriteAllText(customPlugsFilePath, json);
        }

        public string GenerateUniqueCustomTextPlugId()
        {
            return Guid.NewGuid().ToString();
        }

        private void LoadAssetPlugs()
        {
            var extensionsAssetMap = LoadAssets<ExtensionAsset>(ExtensionAssetMapFilename);
            var vsVersionsAssetMap = LoadAssets<VisualStudioVersionAsset>(VsVersionAssetMapFilename);

            _plugs.AddRange(new BaseAssetPlug[]
            {
                new NoneAssetPlug(),
                new ExtensionIconPlug(extensionsAssetMap, _vsObserver),
                new VisualStudioVersionIconPlug(vsVersionsAssetMap, _vsObserver)
            });
        }

        private IAssetMap<T> LoadAssets<T>(string path) where T : Asset
        {
            var assetMap = new OptimizedAssetMap<T>();
            var assetLoader = new JsonAssetsLoader<T>();

            assetMap.Assets = new List<T>(assetLoader.LoadAssets(PathHelper.GetPackageInstallationPath(path)));

            return assetMap;
        }

        private void LoadTextPlugs()
        {
            LoadBuiltInTextPlugs();
            LoadCustomTextPlugs();
        }

        private void LoadBuiltInTextPlugs()
        {
            var localizationService = ServiceRepository.Default.GetService<LocalizationService>();

            _plugs.AddRange(new BaseTextPlug[]
            {
                new NoneTextPlug(),
                new FileNameTextPlug(_vsObserver, localizationService),
                new ProjectNameTextPlug(_vsObserver, localizationService),
                new SolutionNameTextPlug(_vsObserver, localizationService),
                new VisualStudioVersionTextPlug(_vsObserver.DTE)
            });
        }

        private void LoadCustomTextPlugs()
        {
            var customTextPlugsData = GetCustomTextPlugsData();

            var parser = new ObservableStringParser();
            foreach (var customTextPlugInfo in customTextPlugsData)
            {
                var entries = parser.Parse(customTextPlugInfo.Pattern);
                var stringObserver = new StringObserver();
                foreach (var entry in entries)
                {
                    switch (entry.Type)
                    {
                        case ObservableStringParser.EntryType.Text:
                            stringObserver.AddText(entry.Value);
                            break;

                        case ObservableStringParser.EntryType.Keyword:
                            var variable = _variableService.GetVariableByName(entry.Value);
                            if (variable != null)
                                stringObserver.AddText(new ObservableVariable(variable));
                            break;
                    }
                }

                _plugs.Add(new CustomTextPlug(customTextPlugInfo.Id, customTextPlugInfo.Name, stringObserver));
            }
        }

        private void LoadTimerPlugs()
        {
            _plugs.AddRange(new BaseTimerPlug[]
            {
                new NoneTimerPlug(),
                new FileScopeTimerPlug(_vsObserver),
                new ProjectScopeTimerPlug(_vsObserver),
                new SolutionScopeTimerPlug(_vsObserver),
                new ApplicationScopeTimerPlug()
            });
        }

        private void LoadButtonPlugs()
        {
            var gitObserver = ServiceRepository.Default.GetService<GitObserver>();

            _plugs.AddRange(new BaseButtonPlug[]
            {
                new NoneButtonPlug(),
                new GitRepositoryButtonPlug(gitObserver)
            });
        }
    }
}
