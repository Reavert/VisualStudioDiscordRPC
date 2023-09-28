using VisualStudioDiscordRPC.Shared.Localization.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VisualStudioDiscordRPC.Shared.Localization.Interfaces;
using System.Linq;

namespace VisualStudioDiscordRPC.Shared.Localization
{
    public class LocalizationService
    {
        public IReadOnlyList<ILocalizationFile> Localizations => _localizationFiles;
        public ILocalizationFile Current { get; private set; }

        public delegate void LocalizationChangedEventHandler();
        public event LocalizationChangedEventHandler LocalizationChanged;

        private readonly List<ILocalizationFile> _localizationFiles = new List<ILocalizationFile>();

        public LocalizationService(string localizationFolder)
        {
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
                    var acceptableFile = localizationFileFactory.CreateLocalizationFile(filename);
                    _localizationFiles.Add(acceptableFile);
                }
                catch (Exception exception)
                {
                    Debug.Print($"Skipped {filename}: {exception.Message}");
                }
            }

            Current = _localizationFiles.First();
        }

        public ILocalizationFile GetLanguage(string language)
        {
            foreach (ILocalizationFile localizationFile in Localizations)
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

        public string Localize(string key)
        {
            return Current.LocalizedValues[key];
        }
    }
}
