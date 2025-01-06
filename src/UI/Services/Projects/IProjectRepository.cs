using Olve.Utilities.Paginations;

namespace UI.Services.Projects;

public interface IProjectRepository
{
    Task<Result> SetProjectAsync(Project project, CancellationToken ct = default);
    Task<Result> SetProjectAsync(ProjectSummary projectSummary, CancellationToken ct = default);
    Task<Result> SetProjectAsync(Project project, ProjectSummary projectSummary, CancellationToken ct = default);
    Task<Result<Project>> GetProjectAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result<ProjectSummary>> GetProjectSummaryAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result> DeleteProjectAsync(Id<Project> projectId, CancellationToken ct = default);
    Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        OrderKey orderKey = OrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default);
    
    
}