namespace VisualStudioDiscordRPC.Shared.Localization.Models
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Interfaces;

    public class LocalizationManager<T> : ILocalizationManager<T> where T : ILocalizationFile
    {
        public IList<T> Localizations { get; private set; }
        public T Current { get; private set; }

        public delegate void LocalizationChangedEventHandler();
        public event LocalizationChangedEventHandler LocalizationChanged;

        public LocalizationManager(string localizationFolder)
        {
            Localizations = new List<T>();

            string[] localizationFiles = Directory.GetFiles(localizationFolder);
            
            if (localizationFiles.Length == 0)
            {
                throw new Exception($"Localization folder ({localizationFolder}) is empty");
            }

            var localizationFileFactory = new LocalizationFileFactory();

            foreach (string filename in localizationFiles)
            {
                try
                {
                    var acceptableFile = (T)localizationFileFactory.CreateLocalizationFile(filename);
                    Localizations.Add(acceptableFile);
                }
                catch (Exception exception)
                {
                    Debug.Print($"Skipped {filename}: {exception.Message}");
                }
            }

            Current = Localizations[0];
        }

        public T GetLanguage(string language)
        {
            foreach (T localizationFile in Localizations)
            {
                if (localizationFile.LanguageName == language)
                {
                    return localizationFile;
                }
            }

            throw new Exception($"Localization file for the \"{language}\" language not found");
        }

        public void SelectLanguage(string language)
        {
            Current = GetLanguage(language);
            LocalizationChanged?.Invoke();
        }
    }
}
