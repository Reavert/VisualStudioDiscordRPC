namespace VisualStudioDiscordRPC.Shared.Services.Interfaces
{
    public interface IServiceRepository
    {
        T GetService<T>() where T : class;
        void AddService<T>(T service) where T : class;
        void RemoveService<T>(T service) where T : class;
    }
}
