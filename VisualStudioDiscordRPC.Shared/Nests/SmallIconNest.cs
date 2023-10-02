using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class SmallIconNest : BaseDiscordRpcNest<AssetInfo>
    {
        public SmallIconNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.SmallImageKey = data.Key;
            RichPresence.Assets.SmallImageText = data.Description;
        }
    }
}
