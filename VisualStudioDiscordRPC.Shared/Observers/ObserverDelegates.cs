using EnvDTE;

namespace VisualStudioDiscordRPC.Shared.Observers
{
    public delegate void WindowChangedHandler(Window document);
    public delegate void DocumentChangedHandler(Document document);
    public delegate void ProjectChangedHandler(Project project);
    public delegate void SolutionChangedHandler(Solution solution);
}
