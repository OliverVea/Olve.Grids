﻿using Olve.Utilities.Operations;
using Olve.Utilities.Paginations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class ListProjectSummariesOperation(IProjectSearchingRepository projectRepository) : IAsyncOperation<
    ListProjectSummariesOperation.Request,
    ListProjectSummariesOperation.Response>
{
    public record Request(string SearchPrompt, Pagination Pagination);

    public record Response(PaginatedResult<ProjectSummary> Projects);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var result = await projectRepository.SearchProjectSummariesAsync(request.SearchPrompt,
            request.Pagination,
            ProjectOrderKey.LastAccessedDate,
            true,
            ct);

        if (result.TryPickProblems(out var problems, out var projects))
        {
            return problems;
        }

        return new Response(projects);
    }
}