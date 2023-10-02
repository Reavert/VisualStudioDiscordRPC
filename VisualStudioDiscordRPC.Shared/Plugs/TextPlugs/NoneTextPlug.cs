namespace VisualStudioDiscordRPC.Shared.Plugs.TextPlugs
{
    public class NoneTextPlug : BaseTextPlug
    {
        public override void Enable()
        { }

        public override void Disable()
        { }

        protected override string GetData()
        {
            return null;
        }
    }
}
