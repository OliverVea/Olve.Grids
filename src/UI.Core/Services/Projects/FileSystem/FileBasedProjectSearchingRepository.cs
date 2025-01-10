using Olve.Utilities.Paginations;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects.FileSystem;

public class FileBasedProjectSearchingRepository : IProjectSearchingRepository
{
    public Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        ProjectOrderKey projectOrderKey = ProjectOrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default) =>
        Task.FromResult(SearchProjectSummaries(searchPrompt, pagination, projectOrderKey, descending));

    private Result<PaginatedResult<ProjectSummary>> SearchProjectSummaries(string searchPrompt,
        Pagination pagination,
        ProjectOrderKey projectOrderKey = ProjectOrderKey.ProjectName,
        bool descending = false)
    {
        var files = PathHelper
            .GetProjectSummaryFilePaths()
            .ToArray();

        var summaries = files
            .Select(ProjectSummaryFileHelper.Load)
            .ToArray();

        if (summaries.HasProblems())
        {
            var problems = summaries
                .GetProblems()
                .ToArray();

            return Result<PaginatedResult<ProjectSummary>>.Failure(problems);
        }

        var projectSummaries = summaries.GetValues();

        projectSummaries = ApplySearchFiltering(projectSummaries, searchPrompt)
            .ToArray();

        var totalCount = projectSummaries.Count();

        projectSummaries = ApplyOrdering(projectSummaries, projectOrderKey, descending);
        projectSummaries = ApplyPagination(projectSummaries, pagination);

        return new PaginatedResult<ProjectSummary>(projectSummaries.ToArray(), pagination, totalCount);
    }

    private static IEnumerable<ProjectSummary> ApplySearchFiltering(IEnumerable<ProjectSummary> projectSummaries,
        string searchPrompt)
    {
        return projectSummaries.Where(x =>
            x
                .ProjectId.ToString()
                .StartsWith(searchPrompt, StringComparison.InvariantCultureIgnoreCase)
            || x.Name.Value.Contains(searchPrompt, StringComparison.InvariantCultureIgnoreCase));
    }

    private static IEnumerable<ProjectSummary> ApplyOrdering(IEnumerable<ProjectSummary> projectSummaries,
        ProjectOrderKey projectOrderKey,
        bool descending)
    {
        if (projectOrderKey == ProjectOrderKey.None)
        {
            return projectSummaries;
        }

        IComparer<ProjectSummary> orderingFunc = projectOrderKey switch
        {
            ProjectOrderKey.ProjectName => ProjectNameComparer.Shared,
            ProjectOrderKey.LastAccessedDate => LastAccessedComparer.Shared,
            ProjectOrderKey.None => throw new ApplicationException("Comparer not found for OrderKey.None"),
            _ => throw new ArgumentOutOfRangeException(nameof(projectOrderKey), projectOrderKey, null),
        };

        return descending
            ? projectSummaries.OrderDescending(orderingFunc)
            : projectSummaries.Order(orderingFunc);
    }

    private static IEnumerable<ProjectSummary> ApplyPagination(IEnumerable<ProjectSummary> projectSummaries,
        Pagination pagination) =>
        projectSummaries
            .Skip(pagination.Page * pagination.PageSize)
            .Take(pagination.PageSize);

}