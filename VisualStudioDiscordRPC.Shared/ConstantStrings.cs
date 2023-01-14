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

        public const string NewVersionNotification = 
            "A new version of the Visual Studio Discord Rich Presence extension has been installed: v{0}\r\n\r\n" +
            "This version has undergone critical changes that may reset the extension settings.\r\n\r\n" +
            "Please check the extension settings. (Extensions -> Visual Studio Discord RPC).\r\n\r\n" +
            "You can also disable extension update alerts by unchecking the \"Notify about extension updates\" option.";
    }
}
