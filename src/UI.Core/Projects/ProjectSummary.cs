namespace UI.Core.Projects;

public record ProjectSummary(Id<Project> ProjectId, ProjectName Name, DateTimeOffset LastAccessed);