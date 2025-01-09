using Olve.Utilities.Paginations;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects.FileSystem;

public class FileBasedProjectSearchingRepository : IProjectSearchingRepository
{
    public Task<Result<PaginatedResult<ProjectSummary>>> SearchProjectSummariesAsync(
        string searchPrompt,
        Pagination pagination,
        OrderKey orderKey = OrderKey.ProjectName,
        bool descending = false,
        CancellationToken ct = default) =>
        Task.FromResult(SearchProjectSummaries(searchPrompt, pagination, orderKey, descending));

    private Result<PaginatedResult<ProjectSummary>> SearchProjectSummaries(string searchPrompt,
        Pagination pagination,
        OrderKey orderKey = OrderKey.ProjectName,
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

        projectSummaries = ApplyOrdering(projectSummaries, orderKey, descending);
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
        OrderKey orderKey,
        bool descending)
    {
        if (orderKey == OrderKey.None)
        {
            return projectSummaries;
        }

        IComparer<ProjectSummary> orderingFunc = orderKey switch
        {
            OrderKey.ProjectName => ProjectNameComparer.Shared,
            OrderKey.LastAccessedDate => LastAccessedComparer.Shared,
            OrderKey.None => throw new ApplicationException("Comparer not found for OrderKey.None"),
            _ => throw new ArgumentOutOfRangeException(nameof(orderKey), orderKey, null),
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