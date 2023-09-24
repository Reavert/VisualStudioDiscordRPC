using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared
{
    public interface IStringCollectionProvider
    {
        IReadOnlyCollection<string> Items { get; }
        void Add(string item);
        void Remove(string item);
    }
}
