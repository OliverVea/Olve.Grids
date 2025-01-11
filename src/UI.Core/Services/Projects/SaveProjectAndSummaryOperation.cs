using Olve.Utilities.Operations;
using UI.Core.Logging;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects;

public class SaveProjectAndSummaryOperation(
    LoggingService loggingService,
    IProjectGettingRepository projectGettingRepository,
    IProjectSettingRepository projectSettingRepository)
    : IAsyncOperation<SaveProjectAndSummaryOperation.Request>
{
    public record Request(Project Project);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var projectToSave = request.Project with
        {
            LastAccessedAt = DateTimeOffset.Now,
        };

        var projectPathResult = await projectGettingRepository.GetProjectPathAsync(projectToSave.Id, ct);
        if (!projectPathResult.TryPickValue(out var projectPath, out var problems))
        {
            return problems;
        }

        var projectSummary = ProjectMapper.ToProjectSummary(projectToSave, projectPath);

        var saveSummaryResult = await projectSettingRepository.SetProjectSummaryAsync(projectSummary, ct);
        if (saveSummaryResult.TryPickProblems(out problems))
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

        loggingService.Log(new LogMessage(null,
            LogLevel.Information,
            "Saved project with id {0}, name {1}",
            null,
            null,
            projectToSave.Id,
            projectToSave.Name));
        return Result.Success();
    }
}