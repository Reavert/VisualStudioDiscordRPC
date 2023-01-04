using EnvDTE;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Observers;

namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class VisualStudioVersionIconSlot : AssetSlot
    { 
        private readonly IAssetMap<VisualStudioVersionAsset> _assetMap;
        private VsObserver _vsObserver;

        private Dictionary<string, string> _vsVersions = new Dictionary<string, string>
        {
            { "16", "2019" },
            { "17", "2022" }
        };

        public VisualStudioVersionIconSlot(IAssetMap<VisualStudioVersionAsset> assetMap, VsObserver vsObserver) 
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
            _vsObserver.DocumentChanged-= OnDocumentChanged;
        }

        private void OnDocumentChanged(Document document)
        {
            if (document == null)
            {
                return;
            }

            string majorVersion = document.DTE.Version.Split('.')[0];

            var vsVersionIconAsset = _assetMap.GetAsset(asset => asset.Version == majorVersion);

            var assetInfo = new AssetInfo()
            {
                Key = vsVersionIconAsset.Key,
                Description = string.Format(ConstantStrings.VisualStudioVersion, _vsVersions[majorVersion])
            };

            PerformUpdate(assetInfo);
        }
    }
}
