using System.Collections.Generic;
using System.Linq;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Loaders;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Slots;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Services.Models
{
    public class SlotService
    {
        private List<AssetSlot> _assetSlots;
        public IReadOnlyList<AssetSlot> AssetSlots => _assetSlots;

        private List<TextSlot> _textSlots;
        public IReadOnlyList<TextSlot> TextSlots => _textSlots;

        public SlotService()
        {
            CreateSlots();
        }

        public void InitSlotsSubscriptions()
        {
            foreach (AssetSlot assetSlot in _assetSlots)
            {
                assetSlot.Enable();
            }

            foreach (TextSlot textSlot in _textSlots)
            {
                textSlot.Enable();
            }
        }

        public void ClearSlotsSubscriptions()
        {
            foreach (AssetSlot assetSlot in _assetSlots)
            {
                assetSlot.Disable();
            }

            foreach (TextSlot textSlot in _textSlots)
            {
                textSlot.Disable();
            }
        }

        public AssetSlot GetAssetSlotByName(string name)
        {
            return _assetSlots.FirstOrDefault(slot => slot.GetType().Name == name);
        }

        public TextSlot GetTextSlotByName(string name)
        {
            return _textSlots.FirstOrDefault(slot => slot.GetType().Name == name);
        }

        private void CreateSlots()
        {
            const string extensionAssetMapFilename = "extensions_assets_map.json";
            const string vsVersionAssetMapFilename = "vs_assets_map.json";

            var extensionsAssetMap = LoadAssets<ExtensionAsset>(extensionAssetMapFilename);
            var vsVersionsAssetMap = LoadAssets<VisualStudioVersionAsset>(vsVersionAssetMapFilename);

            VsObserver vsObserver = ServiceRepository.Default.GetService<VsObserver>();

            _assetSlots = new List<AssetSlot>
            {
                new NoneAssetSlot(),
                new ExtensionIconSlot(extensionsAssetMap, vsObserver),
                new VisualStudioVersionIconSlot(vsVersionsAssetMap, vsObserver)
            };

            _textSlots = new List<TextSlot>
            {
                new NoneTextSlot(),
                new FileNameSlot(vsObserver),
                new ProjectNameSlot(vsObserver)
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
