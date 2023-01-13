using EnvDTE;
using System.Collections.Generic;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Slots.AssetSlots
{
    public class VisualStudioVersionIconSlot : AssetSlot
    { 
        private readonly IAssetMap<VisualStudioVersionAsset> _assetMap;
        private VsObserver _vsObserver;

        private Solution _solution;

        public VisualStudioVersionIconSlot(IAssetMap<VisualStudioVersionAsset> assetMap, VsObserver vsObserver) 
        {
            _assetMap = assetMap;
            _vsObserver = vsObserver;
        }

        public override void Enable()
        {
            _vsObserver.SolutionChanged += OnSolutionChanged;
        }

        public override void Disable()
        {
            _vsObserver.SolutionChanged -= OnSolutionChanged;
        }

        private void OnSolutionChanged(Solution solution)
        {
            _solution = solution;
            Update();
        }

        protected override AssetInfo GetData()
        {
            if (_solution == null)
            {
                return default;
            }

            string majorVersion = _solution.DTE.Version.Split('.')[0];

            var vsVersionIconAsset = _assetMap.GetAsset(asset => asset.Version == majorVersion);

            var assetInfo = new AssetInfo(
                vsVersionIconAsset.Key,
                string.Format(ConstantStrings.VisualStudioVersion, _solution.DTE.Edition, VisualStudioHelper.GetVersionByDevNumber(_solution.DTE.Version)));

            return assetInfo;
        }
    }
}
