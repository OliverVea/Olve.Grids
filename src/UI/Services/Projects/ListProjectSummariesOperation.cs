using Olve.Utilities.Operations;
using Olve.Utilities.Paginations;

namespace UI.Services.Projects;

public class ListProjectSummariesOperation(IProjectRepository projectRepository) : IAsyncOperation<
    ListProjectSummariesOperation.Request,
    Result<ListProjectSummariesOperation.Response>>
{
    public record Request(Pagination Pagination);

    public record Response(PaginatedResult<ProjectSummary> Projects);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var result = await projectRepository.ListProjectSummariesAsync(request.Pagination, ct);
        if (!result.TryPickValue(out var projects, out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(projects);
    }
}