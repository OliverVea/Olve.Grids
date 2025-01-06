namespace UI.Services.Projects;

public record ProjectSummary(Id<Project> ProjectId, DateTimeOffset LastAccessed, ProjectName Name, ProjectPath Path);
