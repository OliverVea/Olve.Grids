using Olve.Utilities.Paginations;

namespace UI.Services.Projects.Repositories;

public interface IProjectSearchingRepository
{
    Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        OrderKey orderKey = OrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default);
}