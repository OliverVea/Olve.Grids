namespace UI.Core.Projects;

public static class ProjectMapper
{
    public static ProjectSummary ToProjectSummary(Project project, ProjectPath projectPath) =>
        new(project.Id, project.Name, projectPath, project.LastAccessedAt);
}