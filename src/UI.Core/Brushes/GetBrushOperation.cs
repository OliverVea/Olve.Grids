using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Brushes;

public class GetBrushOperation(GetCurrentProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<GetBrushOperation.Request, GetBrushOperation.Response>
{
    public record Request(BrushId BrushId);

    public record Response(ProjectBrush Brush);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetCurrentProjectOperation.Request currentProjectRequest = new();
        var projectResult = await getCurrentProjectOperation.ExecuteAsync(currentProjectRequest, ct);
        if (projectResult.TryPickProblems(out var problems, out var projectResponse))
        {
            return problems;
        }

        var brushes = projectResponse.Project.Brushes;
        if (brushes.TryGetValue(request.BrushId, out var brush))
        {
            return new Response(brush);
        }

        return new ResultProblem("Brush with id '{0}' was not found in project", request.BrushId);
    }
}