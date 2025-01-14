using Olve.Utilities.Operations;
using UI.Core.Services.Projects;

namespace UI.Core.Services.Brushes;

public class ListBrushesOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<ListBrushesOperation.Request, ListBrushesOperation.Response>
{
    public record Request;

    public record Response(IEnumerable<ProjectBrush> Brushes);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var currentProjectResult = await currentProjectRepository.GetCurrentProjectAsync(ct);
        if (currentProjectResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        return new Response(project.Brushes.Values);
    }
}