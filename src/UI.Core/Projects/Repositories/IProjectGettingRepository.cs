namespace UI.Core.Projects.Repositories;

public interface IProjectGettingRepository
{
    Task<Result<Project>> GetProjectAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result<ProjectSummary>> GetProjectSummaryAsync(Id<Project> projectId, CancellationToken ct = default);
}