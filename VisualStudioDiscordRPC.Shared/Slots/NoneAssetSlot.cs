namespace VisualStudioDiscordRPC.Shared.Slots
{
    public class NoneAssetSlot : AssetSlot
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override AssetInfo GetData()
        {
            return new AssetInfo()
            {
                Key = string.Empty,
                Description = string.Empty
            };
        }
    }
}
