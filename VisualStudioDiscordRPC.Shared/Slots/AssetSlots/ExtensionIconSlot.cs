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
        private readonly IAssetMap<ExtensionAsset> _assetMap;
        private readonly VsObserver _vsObserver;

        private Document _document;

        public ExtensionIconSlot(IAssetMap<ExtensionAsset> assetMap, VsObserver vsObserver)
        {
            _assetMap = assetMap;
            _vsObserver = vsObserver;
            _document = _vsObserver.DTE.ActiveDocument;
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
            _document = document;

            Update();
        }

        protected override AssetInfo GetData()
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            ExtensionAsset suitableAsset = null;

            if (_document == null)
            {
                return AssetInfo.Idle;
            }

            string extension = Path.GetExtension(_document.Name);
            suitableAsset = _assetMap.GetAsset(asset => asset.Extensions.Contains(extension));
            
            if (suitableAsset == null)
            {
                suitableAsset = ExtensionAsset.Unknown;
            }

            var assetInfo = new AssetInfo(suitableAsset.Key, suitableAsset.Name);

            return assetInfo;
        }
    }
}
