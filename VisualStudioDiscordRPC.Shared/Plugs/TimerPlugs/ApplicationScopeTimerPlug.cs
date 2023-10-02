namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public class ApplicationScopeTimerPlug : BaseTimerPlug
    {
        public ApplicationScopeTimerPlug() : base()
        {
            // Never change timestamp for application scope.
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
