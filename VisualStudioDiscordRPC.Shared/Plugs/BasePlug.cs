namespace VisualStudioDiscordRPC.Shared.Plugs
{
    public abstract class BasePlug
    {
        public abstract void Update();

        public abstract void Enable();

        public abstract void Disable();

        public virtual string GetId()
        {
            return GetType().Name;
        }
    }
}
