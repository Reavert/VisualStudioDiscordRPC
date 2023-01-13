using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Updaters.Base;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class SmallIconUpdater : BaseDiscordRpcUpdater<AssetInfo>
    {
        public SmallIconUpdater(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.SmallImageKey = data.Key;
            RichPresence.Assets.SmallImageText = data.Description;
        }
    }
}
