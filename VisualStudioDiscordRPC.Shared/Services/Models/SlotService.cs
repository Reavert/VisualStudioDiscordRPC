using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Slots;
using VisualStudioDiscordRPC.Shared.Slots.AssetSlots;
using VisualStudioDiscordRPC.Shared.Slots.ButtonSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots;
using VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom;
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class SlotService
    {
        private const string ExtensionAssetMapFilename = "extensions_assets_map.json";
        private const string VsVersionAssetMapFilename = "vs_assets_map.json";

        private List<BaseSlot> _slots = new List<BaseSlot>();

        private readonly IAssetMap<ExtensionAsset> _extensionsAssetMap;
        private readonly IAssetMap<VisualStudioVersionAsset> _vsVersionsAssetMap;

        private readonly VsObserver _vsObserver = ServiceRepository.Default.GetService<VsObserver>();
        private readonly MacroService _macroService = ServiceRepository.Default.GetService<MacroService>();
        public SlotService()
        {
            _extensionsAssetMap = LoadAssets<ExtensionAsset>(ExtensionAssetMapFilename);
            _vsVersionsAssetMap = LoadAssets<VisualStudioVersionAsset>(VsVersionAssetMapFilename);

            LoadSlots();
        }

        public void LoadSlots()
        {
            LoadAssetSlots();
            LoadTextSlots();
            LoadTimerSlots();
            LoadButtonSlots();
        }

        public void InitSlotsSubscriptions()
        {
            foreach (BaseSlot slot in _slots)
            {
                slot.Enable();
            }
        }

        public void ClearSlotsSubscriptions()
        {
            foreach (BaseSlot slot in _slots)
            {
                slot.Disable();
            }
        }

        public TSlot GetSlotByName<TSlot>(string name) where TSlot : BaseSlot
        {
            return (TSlot) _slots.FirstOrDefault(slot => slot.GetType().Name == name);
        }

        public IReadOnlyList<TSlot> GetSlotsOfType<TSlot>() where TSlot : BaseSlot
        {
            return _slots
                .Where(slot => slot is TSlot)
                .Select(slot => (TSlot) slot)
                .ToList();
        }

        private IAssetMap<T> LoadAssets<T>(string path) where T : Asset
        {
            var assetMap = new OptimizedAssetMap<T>();
            var assetLoader = new JsonAssetsLoader<T>();

            assetMap.Assets = new List<T>(assetLoader.LoadAssets(PathHelper.GetPackageInstallationPath(path)));

            return assetMap;
        }

        private void LoadAssetSlots()
        {
            _slots.AddRange(new AssetSlot[]
            {
                new NoneAssetSlot(),
                new ExtensionIconSlot(_extensionsAssetMap, _vsObserver),
                new VisualStudioVersionIconSlot(_vsVersionsAssetMap, _vsObserver)
            });
        }

        private void LoadTextSlots()
        {
            var text = "{solution_name} - {project_name} - {file_name}";
            var parser = new ObservableStringParser();
            var entries = parser.Parse(text);

            var stringObserver = new StringObserver();
            foreach (var entry in entries)
            {
                switch (entry.Type)
                {
                    case ObservableStringParser.EntryType.Text:
                        stringObserver.AddText(entry.Value);
                        break;

                    case ObservableStringParser.EntryType.Keyword:
                        var macro = _macroService.GetMacroByName(entry.Value);
                        stringObserver.AddText(new ObservableMacro(macro));
                        break;
                }
            }

            _slots.Add(new CustomTextSlot(stringObserver));
        }

        private void LoadTimerSlots()
        {
            _slots.AddRange(new TimerSlot[]
            {
                new NoneTimerSlot(),
                new WithinFilesTimerSlot(_vsObserver),
                new WithinProjectsTimerSlot(_vsObserver),
                new WithinSolutionsTimerSlot(_vsObserver),
                new WithinApplicationTimerSLot()
            });
        }

        private void LoadButtonSlots()
        {
            _slots.AddRange(new ButtonSlot[]
            {
                new NoneButtonSlot(),
                new GitRepositoryButtonSlot(_vsObserver)
            });
        }
    }
}
