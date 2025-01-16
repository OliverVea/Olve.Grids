using UI.Core.Projects;

namespace UI.Core;

public static class Events
{
    public static event Action<Id<Project>>? ProjectChanged;

    public static void OnProjectChanged(Id<Project> projectId) => ProjectChanged?.Invoke(projectId);
}