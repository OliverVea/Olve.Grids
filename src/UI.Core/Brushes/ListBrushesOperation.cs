using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Brushes;

public class ListBrushesOperation(GetProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<ListBrushesOperation.Request, ListBrushesOperation.Response>
{
    public record Request(Id<Project> ProjectId);

    public record Response(IEnumerable<ProjectBrush> Brushes);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetProjectOperation.Request currentProjectRequest = new(request.ProjectId);
        var currentProjectResult = await getCurrentProjectOperation.ExecuteAsync(currentProjectRequest, ct);
        if (currentProjectResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        return new Response(project.Brushes.Values);
    }
}