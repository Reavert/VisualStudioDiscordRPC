namespace VisualStudioDiscordRPC.Shared.Localization.Interfaces
{
    using System.Collections.Generic;

    public interface ILocalizationManager<T> where T : ILocalizationFile
    {
        IList<T> Localizations { get; }
        T Current { get; }

        T GetLanguage(string language);
        void SelectLanguage(string language);
    }
}
