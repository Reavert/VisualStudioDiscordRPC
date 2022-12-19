namespace VisualStudioDiscordRPC.Shared.Observers
{
    public interface IObserver
    {
        void Observe();
        void Unobserve();
    }
}
