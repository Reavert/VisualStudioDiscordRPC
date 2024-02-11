using DiscordRPC;
using System.Text;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class LargeIconNest : BaseDiscordRpcNest<AssetInfo>
    {
        public LargeIconNest(RichPresence richPresence) : base(richPresence) 
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.LargeImageKey = StringHelper.ReEncodeWithMaxLength(data.Key, 256, Encoding.UTF8);
            RichPresence.Assets.LargeImageText = StringHelper.ReEncodeWithMaxLength(data.Description, 128, Encoding.UTF8);
        }
    }
}
