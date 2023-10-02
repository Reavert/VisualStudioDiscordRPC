namespace VisualStudioDiscordRPC.Shared.Plugs.TimerPlugs
{
    public class NoneTimerPlug : BaseTimerPlug
    {
        public NoneTimerPlug() 
        {
            ClearTimestamp();
        }

        public override void Enable()
        { }

        public override void Disable()
        { }
    }
}
