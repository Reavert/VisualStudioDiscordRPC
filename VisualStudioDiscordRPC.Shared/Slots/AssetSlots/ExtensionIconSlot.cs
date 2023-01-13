using EnvDTE;
using System.IO;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots.AssetSlots
{
    public class ExtensionIconSlot : AssetSlot
    {
        private IAssetMap<ExtensionAsset> _assetMap;
        private VsObserver _vsObserver;

        private Document _document;

        public ExtensionIconSlot(IAssetMap<ExtensionAsset> assetMap, VsObserver vsObserver)
        {
            _assetMap = assetMap;
            _vsObserver = vsObserver;
        }

        public override void Enable()
        {
            _vsObserver.DocumentChanged += OnDocumentChanged;
        }

        public override void Disable()
        {
            _vsObserver.DocumentChanged -= OnDocumentChanged;
        }

        private void OnDocumentChanged(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            _document = document;
            Update();
        }

        protected override AssetInfo GetData()
        {
            ExtensionAsset suitableAsset = null;

            if (_document == null)
            {
                return default;
            }

            string extension = Path.GetExtension(_document.Name);
            suitableAsset = _assetMap.GetAsset(asset => asset.Extensions.Contains(extension));
            
            if (suitableAsset == null)
            {
                suitableAsset = ExtensionAsset.Default;
            }

            var assetInfo = new AssetInfo(suitableAsset.Key, suitableAsset.Name);

            return assetInfo;
        }
    }
}
