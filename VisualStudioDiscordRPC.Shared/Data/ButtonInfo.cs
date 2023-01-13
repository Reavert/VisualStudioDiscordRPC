namespace VisualStudioDiscordRPC.Shared.Data
{
    public struct ButtonInfo
    {
        public static ButtonInfo None = new ButtonInfo(string.Empty, string.Empty);

        public readonly string Label;
        public readonly string Url;

        public ButtonInfo(string caption, string url)
        {
            Label = caption;
            Url = url;
        }
    }
}
