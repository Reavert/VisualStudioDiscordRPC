namespace VisualStudioDiscordRPC.Shared.Localization.Interfaces
{
    using System.Collections.Generic;

    public interface ILocalizationFile
    {
        string LanguageName { get; }
        string LocalizedLanguageName { get; }
        IReadOnlyDictionary<string, string> LocalizedValues { get; }
    }
}
