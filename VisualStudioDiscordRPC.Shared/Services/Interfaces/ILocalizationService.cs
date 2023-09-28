using VisualStudioDiscordRPC.Shared.Localization.Interfaces;

namespace VisualStudioDiscordRPC.Shared.Services.Interfaces
{
    using System.Collections.Generic;

    public interface ILocalizationService
    {
        IReadOnlyList<ILocalizationFile> Localizations { get; }
        ILocalizationFile Current { get; }

        ILocalizationFile GetLanguage(string language);
        void SelectLanguage(string language);
    }
}
