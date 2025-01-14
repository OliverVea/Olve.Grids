using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Brushes;

public class ListBrushesOperation(GetCurrentProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<ListBrushesOperation.Request, ListBrushesOperation.Response>
{
    public record Request;

    public record Response(IEnumerable<ProjectBrush> Brushes);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetCurrentProjectOperation.Request currentProjectRequest = new();
        var currentProjectResult = await getCurrentProjectOperation.ExecuteAsync(currentProjectRequest, ct);
        if (currentProjectResult.TryPickProblems(out var problems, out var projectResponse))
        {
            return problems;
        }

        return new Response(projectResponse.Project.Brushes.Values);
    }
}