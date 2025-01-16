using Olve.Utilities.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class GetProjectOperation(IProjectGettingRepository projectGettingRepository, ProjectCache projectCache)
    : IAsyncOperation<GetProjectOperation.Request, Project>
{
    public readonly record struct Request(Id<Project> ProjectId);

    public async Task<Result<Project>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var cacheResult = projectCache.GetProject(request.ProjectId);
        if (cacheResult.TryPickT0(out var project, out _))
        {
            return project;
        }

        var projectResult = await projectGettingRepository.GetProjectAsync(request.ProjectId, ct);
        if (projectResult.TryPickProblems(out var problems, out project))
        {
            return problems;
        }

        projectCache.AddProject(project);

        return project;
    }
}