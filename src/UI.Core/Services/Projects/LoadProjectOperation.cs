using Olve.Utilities.Operations;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects;

public class LoadProjectOperation(IProjectGettingRepository projectGettingRepository)
    : IAsyncOperation<LoadProjectOperation.Request, LoadProjectOperation.Response>
{
    public record Request(Id<Project> ProjectId);

    public record Response(Project Project);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var getResult = await projectGettingRepository.GetProjectAsync(request.ProjectId, ct);

        if (getResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        return new Response(project);
    }
}