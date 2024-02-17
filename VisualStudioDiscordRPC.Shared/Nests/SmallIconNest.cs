using DiscordRPC;
using System.Text;
using VisualStudioDiscordRPC.Shared.Data;
using VisualStudioDiscordRPC.Shared.Nests.Base;
using VisualStudioDiscordRPC.Shared.Utils;

namespace VisualStudioDiscordRPC.Shared.Nests
{
    public class SmallIconNest : BaseDiscordRpcNest<AssetInfo>
    {
        public SmallIconNest(RichPresence richPresence) : base(richPresence)
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.SmallImageKey = StringHelper.RecodeToFitMaxLength(data.Key, 256, Encoding.UTF8);
            RichPresence.Assets.SmallImageText = StringHelper.RecodeToFitMaxLength(data.Description, 128, Encoding.UTF8);
        }
    }
}
