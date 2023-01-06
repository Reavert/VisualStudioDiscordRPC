using System;

namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    using System.Collections.Generic;
    using Interfaces;

    public abstract class LocalizationFile : ILocalizationFile
    {
        private static string EnumerationSeparator = ".";

        public IDictionary<string, string> LocalizedValues { get; protected set; }

        public string LanguageName { get; protected set; }

        public string LocalizedLanguageName { get; protected set; }

        #region Rich Presence localization fields

        public string Project => LocalizedValues["project"];
        public string File => LocalizedValues["file"];
        public string Solution => LocalizedValues["solution"];

        public string NoActiveProject => LocalizedValues["noActiveProject"];
        public string NoActiveFile => LocalizedValues["noActiveFile"];
        public string NoActiveSolution => LocalizedValues["noActiveSolution"];

        #endregion

        public string GetTypeValue(Type enumType, object value)
        {
            string nestedTypeName = enumType.Name;
            string index = nestedTypeName + EnumerationSeparator + value;

            return LocalizedValues[index];
        }
    }
}
