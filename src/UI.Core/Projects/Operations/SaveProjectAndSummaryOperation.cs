using Olve.Utilities.Operations;
using UI.Core.Logging;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class SaveProjectAndSummaryOperation(
    LoggingService loggingService,
    IProjectSettingRepository projectSettingRepository)
    : IAsyncOperation<SaveProjectAndSummaryOperation.Request>
{
    public record Request(Project Project);

    public class Factory(IServiceProvider serviceProvider)
        : AsyncOperationFactory<SaveProjectAndSummaryOperation, Request>(serviceProvider);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var projectToSave = request.Project with
        {
            LastChangedAt = DateTimeOffset.Now,
        };

        var projectSummary = ProjectMapper.ToProjectSummary(projectToSave);

        var saveSummaryResult = await projectSettingRepository.SetProjectSummaryAsync(projectSummary, ct);
        if (saveSummaryResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        var saveResult = await projectSettingRepository.SetProjectAsync(projectToSave, ct);
        if (saveResult.TryPickProblems(out problems))
        {
            var problem = new ResultProblem("Failed to save project");
            var newProblems = problems
                .Prepend(problem)
                .ToArray();

            return Result.Failure(newProblems);
        }

        loggingService.Log(new LogMessage("Saved project with id {0}, name {1}",
            projectToSave.Id,
            projectToSave.Name));
        return Result.Success();
    }
}