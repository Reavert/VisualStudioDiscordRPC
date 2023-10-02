using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class LargeIconNest : BaseDiscordRpcNest<AssetInfo>
    {
        public LargeIconNest(RichPresence richPresence) : base(richPresence) 
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.LargeImageKey = data.Key;
            RichPresence.Assets.LargeImageText = data.Description;
        }
    }
}
