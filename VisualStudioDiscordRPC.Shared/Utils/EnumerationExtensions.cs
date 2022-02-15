using System;
using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.Utils
{
    public static class EnumerationExtensions
    {
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            int index = 0;
            foreach (T item in items)
            {
                if (predicate(item))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }
}
