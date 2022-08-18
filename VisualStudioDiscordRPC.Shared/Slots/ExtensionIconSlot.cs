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

        public ExtensionIconSlot(IAssetMap<ExtensionAsset> assetMap, IObserver observer) : 
            base(observer)
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

            ExtensionAsset suitableAsset;

            if (document != null)
            {
                string extension = Path.GetExtension(document.Name);
                suitableAsset = _assetMap.GetAsset(asset => asset.Extensions.Contains(extension));
            }
            else
            {
                suitableAsset = ExtensionAsset.Default;
            }

            PerformUpdate(suitableAsset.Key);
        }
    }
}
