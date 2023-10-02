using EnvDTE;
using VisualStudioDiscordRPC.Shared.AssetMap.Interfaces;
using VisualStudioDiscordRPC.Shared.AssetMap.Models.Assets;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Observers;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Plugs.AssetPlugs
{
    public class VisualStudioVersionIconPlug : BaseAssetPlug
    { 
        private readonly IAssetMap<VisualStudioVersionAsset> _assetMap;
        private readonly VsObserver _vsObserver;

        private Solution _solution;

        public VisualStudioVersionIconPlug(IAssetMap<VisualStudioVersionAsset> assetMap, VsObserver vsObserver) 
        {
            _assetMap = assetMap;
            _vsObserver = vsObserver;
            _solution = _vsObserver.DTE.Solution;
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
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            if (_solution == null)
            {
                return AssetInfo.None;
            }
            
            string majorVersion = _solution.DTE.Version.Split('.')[0];

            VisualStudioVersionAsset vsVersionIconAsset = _assetMap.GetAsset(asset => asset.Version == majorVersion);

            var assetInfo = new AssetInfo(
                vsVersionIconAsset.Key,
                string.Format(ConstantStrings.VisualStudioVersion, _solution.DTE.Edition, VisualStudioHelper.GetVersionByDevNumber(_solution.DTE.Version)));

            return assetInfo;
        }
    }
}
