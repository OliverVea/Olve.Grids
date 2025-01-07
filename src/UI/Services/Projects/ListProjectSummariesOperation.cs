using Olve.Utilities.Operations;
using Olve.Utilities.Paginations;
using UI.Services.Projects.Repositories;

namespace UI.Services.Projects;

public class ListProjectSummariesOperation(IProjectSearchingRepository projectRepository) : IAsyncOperation<
    ListProjectSummariesOperation.Request,
    Result<ListProjectSummariesOperation.Response>>
{
    public record Request(string SearchPrompt, Pagination Pagination);

    public record Response(PaginatedResult<ProjectSummary> Projects);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var result = await projectRepository.SearchProjectSummariesAsync(request.SearchPrompt,
            request.Pagination,
            OrderKey.LastAccessedDate,
            true,
            ct);

        if (!result.TryPickValue(out var projects, out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(projects);
    }
}