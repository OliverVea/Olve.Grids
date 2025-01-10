using Olve.Utilities.Paginations;

namespace UI.Core.Services.Projects.Repositories;

public interface IProjectSearchingRepository
{
    Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        ProjectOrderKey projectOrderKey = ProjectOrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default);
}