using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Repositories;

namespace UI.Core.Brushes;

public class GetBrushOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<GetBrushOperation.Request, GetBrushOperation.Response>
{
    public record Request(BrushId BrushId);

    public record Response(ProjectBrush Brush);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var projectResult = await currentProjectRepository.GetCurrentProjectAsync(ct);
        if (projectResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        if (project.Brushes.TryGetValue(request.BrushId, out var brush))
        {
            return new Response(brush);
        }

        return new ResultProblem("Brush with id '{0}' was not found in project", request.BrushId);
    }
}