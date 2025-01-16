using Olve.Utilities.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class SetProjectOperation(IProjectSettingRepository projectSettingRepository, ProjectCache projectCache)
    : IAsyncOperation<SetProjectOperation.Request>
{
    public record Request(Project Project);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var projectSummary = ProjectMapper.ToProjectSummary(request.Project);

        var saveSummaryResult =
            await projectSettingRepository.SetProjectAndSummaryAsync(request.Project, projectSummary, ct);
        if (saveSummaryResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        projectCache.AddProject(request.Project);

        Events.OnProjectChanged(request.Project.Id);

        return Result.Success();
    }
}