using DiscordRPC;
using VisualStudioDiscordRPC.Shared.Slots;

namespace VisualStudioDiscordRPC.Shared.Updaters
{
    public class LargeIconUpdater : BaseDiscordRpcUpdater<AssetInfo>
    {
        public LargeIconUpdater(RichPresence richPresence) : base(richPresence) 
        { }

        protected override void Update(AssetInfo data)
        {
            RichPresence.Assets.LargeImageKey = data.Key;
            RichPresence.Assets.LargeImageText = data.Description;
        }
    }
}
