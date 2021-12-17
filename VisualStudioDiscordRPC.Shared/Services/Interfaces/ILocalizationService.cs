using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Services.Interfaces
{
    using System.Collections.Generic;

    public interface ILocalizationService<T> where T : ILocalizationFile
    {
        IList<T> Localizations { get; }
        T Current { get; }

        T GetLanguage(string language);
        void SelectLanguage(string language);
    }
}
