using Olve.Utilities.Operations;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects;

public class LoadProjectSummaryOperation(IProjectGettingRepository projectGettingRepository)
    : IAsyncOperation<LoadProjectSummaryOperation.Request, LoadProjectSummaryOperation.Response>
{
    public record Request(Id<Project> ProjectId);

    public record Response(ProjectSummary ProjectSummary);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var getResult = await projectGettingRepository.GetProjectSummaryAsync(request.ProjectId, ct);

        if (getResult.TryPickProblems(out var problems, out var projectSummary))
        {
            return problems;
        }

        return new Response(projectSummary);
    }
}