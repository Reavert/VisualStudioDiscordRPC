using System;
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
using VisualStudioDiscordRPC.Shared.Slots.TimerSlots;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class SlotService
    {
        private List<BaseSlot> _slots;

        public SlotService()
        {
            CreateSlots();
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

        private void CreateSlots()
        {
            const string extensionAssetMapFilename = "extensions_assets_map.json";
            const string vsVersionAssetMapFilename = "vs_assets_map.json";

            var extensionsAssetMap = LoadAssets<ExtensionAsset>(extensionAssetMapFilename);
            var vsVersionsAssetMap = LoadAssets<VisualStudioVersionAsset>(vsVersionAssetMapFilename);

            VsObserver vsObserver = ServiceRepository.Default.GetService<VsObserver>();

            _slots = new List<BaseSlot>
            {
                // Asset slots.
                new NoneAssetSlot(),
                new ExtensionIconSlot(extensionsAssetMap, vsObserver),
                new VisualStudioVersionIconSlot(vsVersionsAssetMap, vsObserver),

                // Text slots.
                new NoneTextSlot(),
                new FileNameSlot(vsObserver),
                new ProjectNameSlot(vsObserver),
                new SolutionNameSlot(vsObserver),
                new VisualStudioVersionTextSlot(vsObserver),

                // Timer slots.
                new NoneTimerSlot(),
                new WithinFilesTimerSlot(vsObserver),
                new WithinProjectsTimerSlot(vsObserver),
                new WithinSolutionsTimerSlot(vsObserver),

                // Button slots.
                new NoneButtonSlot(),
                new GitRepositoryButtonSlot(vsObserver)
            };
        }

        private IAssetMap<T> LoadAssets<T>(string path) where T : Asset
        {
            var assetMap = new OptimizedAssetMap<T>();
            var assetLoader = new JsonAssetsLoader<T>();

            assetMap.Assets = new List<T>(assetLoader.LoadAssets(PackageFileHelper.GetPackageFilePath(path)));

            return assetMap;
        }
    }
}
