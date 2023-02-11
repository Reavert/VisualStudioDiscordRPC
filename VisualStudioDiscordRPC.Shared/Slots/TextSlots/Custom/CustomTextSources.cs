using System.Collections.Generic;

namespace VisualStudioDiscordRPC.Shared.Slots.TextSlots.Custom
{
    public static class CustomTextSources
    {
        private static readonly Dictionary<string, ITextSource> GlobalTextSources = 
            new Dictionary<string, ITextSource>();

        public static void AddGlobalTextSource(ITextSource textSource)
        {
            GlobalTextSources.Add(textSource.Name, textSource);
        }

        public static ITextSource GetGlobalTextSourceByName(string name)
        {
            return GlobalTextSources[name];
        }
    }


}
