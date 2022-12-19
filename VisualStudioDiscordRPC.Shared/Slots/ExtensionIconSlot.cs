using EnvDTE;
using System.IO;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class ExtensionIconSlot : AbstractSlot
    {
        private IAssetMap<ExtensionAsset> _assetMap;
        private VsObserver _vsObserver;

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

            ExtensionAsset suitableAsset = null;

            if (document != null)
            {
                string extension = Path.GetExtension(document.Name);
                suitableAsset = _assetMap.GetAsset(asset => asset.Extensions.Contains(extension));
            }

            if (suitableAsset == null)
            {
                suitableAsset = ExtensionAsset.Default;
            }

            PerformUpdate(suitableAsset.Key);
        }
    }
}
