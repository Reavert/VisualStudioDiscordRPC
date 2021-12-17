namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    using System.Collections.Generic;
    using Interfaces;

    public abstract class LocalizationFile : ILocalizationFile
    {
        public IDictionary<string, string> LocalizedValues { get; protected set; }

        public string LanguageName { get; protected set; }

        public string LocalizedLanguageName { get; protected set; }

        #region Rich Presence localization fields

        public string Project => LocalizedValues["project"];
        public string File => LocalizedValues["file"];
        public string NoActiveProject => LocalizedValues["noActiveProject"];
        public string NoActiveFile => LocalizedValues["noActiveFile"];
        public string FileExtension => LocalizedValues["fileExtension"];
        public string VisualStudioVersion => LocalizedValues["visualStudioVersion"];
        public string None => LocalizedValues["none"];
        public string FileName => LocalizedValues["fileName"];
        public string ProjectName => LocalizedValues["projectName"];

        #endregion
    }
}
