using Olve.Utilities.Operations;

namespace UI.Core.Services.Projects;

public class GetCurrentProjectOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<GetCurrentProjectOperation.Request,
        GetCurrentProjectOperation.Response>
{
    public record Request;

    public record Response(Project Project);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var currentProjectResult = await currentProjectRepository.GetCurrentProjectAsync(ct);

        if (currentProjectResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        if (currentProjectResult.Value is not { } currentProject)
        {
            return Result<Response>.Failure(new ResultProblem("No project is currently selected."));
        }

        return Result<Response>.Success(new Response(currentProject));
    }
}