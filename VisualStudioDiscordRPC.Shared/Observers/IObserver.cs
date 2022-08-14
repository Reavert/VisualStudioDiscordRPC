namespace VisualStudioDiscordRPC.Shared.Observers
{
    public interface IObserver
    {
        event DocumentChangedHandler DocumentChanged;
        event ProjectChangedHandler ProjectChanged;
        event SolutionChangedHandler SolutionChanged;
    }
}
