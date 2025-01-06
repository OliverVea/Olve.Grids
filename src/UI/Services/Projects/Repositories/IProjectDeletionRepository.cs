using Olve.Utilities.Paginations;

namespace UI.Services.Projects.Repositories;

public interface IProjectDeletionRepository
{
    Task<Result> DeleteProjectAsync(Id<Project> projectId, CancellationToken ct = default);
}

public interface IProjectGettingRepository
{
    Task<Result<Project>> GetProjectAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result<ProjectSummary>> GetProjectSummaryAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result<ProjectPath>> GetProjectPathAsync(Id<Project> projectId, CancellationToken ct = default);
}

public interface IProjectSettingRepository
{
    Task<Result> SetProjectAsync(Project project, CancellationToken ct = default);
    Task<Result> SetProjectAsync(ProjectSummary projectSummary, CancellationToken ct = default);
    Task<Result> SetProjectAsync(Project project, ProjectSummary projectSummary, CancellationToken ct = default);
}

public interface IProjectSearchingRepository
{
    Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        OrderKey orderKey = OrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default);
}