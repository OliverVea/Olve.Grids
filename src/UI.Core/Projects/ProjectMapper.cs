namespace UI.Core.Projects;

public static class ProjectMapper
{
    public static ProjectSummary ToProjectSummary(Project project) =>
        new(project.Id, project.Name, project.LastChangedAt);
}