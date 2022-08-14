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

        protected ExtensionIconSlot(IAssetMap<ExtensionAsset> assetMap, IObserver observer, SlotUpdateHandler slotUpdateHandler) : 
            base(observer, slotUpdateHandler)
        {
            _assetMap = assetMap;
        }

        public override void Enable()
        {
            Observer.DocumentChanged += OnDocumentChanged;
        }

        public override void Disable()
        {
            Observer.DocumentChanged -= OnDocumentChanged;
        }

        private void OnDocumentChanged(Document document)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            string extension = Path.GetExtension(document.Name);
            ExtensionAsset suitableAsset = _assetMap.GetAsset(asset => asset.Extensions.Contains(extension));

            SlotUpdateHandler.Update(suitableAsset.Key);
        }
    }
}
