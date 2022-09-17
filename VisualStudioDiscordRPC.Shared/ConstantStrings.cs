namespace VisualStudioDiscordRPC.Shared
{
    public static class ConstantStrings
    {
        /// <summary>
        /// {0} - localized word of "File"<br/>
        /// {1} - file name
        /// </summary>
        public const string ActiveFileFormat = "{0} {1}";

        /// <summary>
        /// {0} - localized word of "Project"<br/>
        /// {1} - project name
        /// </summary>
        public const string ActiveProjectFormat = "{0} {1}";

        /// <summary>
        /// {0} - localized word of "Solution"<br/>
        /// {1} - solution name
        /// </summary>
        public const string ActiveSolutionFormat = "{0} {1}";

        /// <summary>
        /// {0} - Visual Studio edition
        /// {1} - Visual Studio version
        /// </summary>
        public const string VisualStudioVersion = "Visual Studio {0} {1}";

        /// <summary>
        /// {0} - Visual Studio version
        /// </summary>
        public const string VisualStudioVersionAssetKey = "vs_{0}";
    }
}
