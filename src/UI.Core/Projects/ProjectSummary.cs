namespace UI.Core.Projects;

public record ProjectSummary(Id<Project> ProjectId, ProjectName Name, ProjectPath Path, DateTimeOffset LastAccessed);