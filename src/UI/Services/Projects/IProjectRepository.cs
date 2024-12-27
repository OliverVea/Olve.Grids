using Olve.Utilities.Paginations;

namespace UI.Services.Projects;

public interface IProjectRepository
{
    Task<Result<PaginatedResult<ProjectSummary>>> ListProjectSummariesAsync(Pagination pagination,
        CancellationToken ct = default);

    Task<Result<ProjectPath>> CreateProjectAsync(Project project, CancellationToken ct = default);
}