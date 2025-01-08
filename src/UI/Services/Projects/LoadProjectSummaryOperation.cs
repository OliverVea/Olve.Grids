using Olve.Utilities.Operations;
using UI.Services.Projects.Repositories;

namespace UI.Services.Projects;

public class LoadProjectSummaryOperation(IProjectGettingRepository projectGettingRepository)
    : IAsyncOperation<LoadProjectSummaryOperation.Request, Result<LoadProjectSummaryOperation.Response>>
{
    public record Request(Id<Project> ProjectId);

    public record Response(ProjectSummary ProjectSummary);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var getResult = await projectGettingRepository.GetProjectSummaryAsync(request.ProjectId, ct);

        if (!getResult.TryPickValue(out var projectSummary, out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(projectSummary);
    }
}