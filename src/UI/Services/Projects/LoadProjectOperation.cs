using Olve.Utilities.Operations;
using UI.Services.Projects.Repositories;

namespace UI.Services.Projects;

public class LoadProjectOperation(IProjectGettingRepository projectGettingRepository)
    : IAsyncOperation<LoadProjectOperation.Request, Result<LoadProjectOperation.Response>>
{
    public record Request(Id<Project> ProjectId);

    public record Response(Project Project);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var getResult = await projectGettingRepository.GetProjectAsync(request.ProjectId, ct);

        if (!getResult.TryPickValue(out var project, out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(project);
    }
}