namespace VisualStudioDiscordRPC.Shared
{
    public class RichPresenceWrapper
    {
        public enum IconMode
        {
            None,
            VisualStudioVersion,
            FileExtension
        }

        public enum TextMode
        {
            None,
            VisualStudioVersion,
            FileExtension,
            ProjectName,
            FileName
        }

        public TextMode TitleText { get; set; }
        public TextMode SubTitleText { get; set; }

        public bool WorkTimerVisible { get; set; }

        public IconMode LargeIcon { get; set; }
        public IconMode SmallIcon { get; set; }


    }
}
