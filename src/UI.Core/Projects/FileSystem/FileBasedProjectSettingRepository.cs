using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.FileSystem;

public class FileBasedProjectSettingRepository : IProjectSettingRepository
{

    public async Task<Result> SetProjectAndSummaryAsync(Project project,
        ProjectSummary projectSummary,
        CancellationToken ct = default)
    {
        var summaryResult = await SetProjectSummaryAsync(projectSummary, ct);
        if (summaryResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        var projectResult = await SetProjectAsync(project, ct);
        if (projectResult.TryPickProblems(out problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }

    public Task<Result> SetProjectSummaryAsync(ProjectSummary projectSummary, CancellationToken ct = default) =>
        Task.FromResult(SetProjectSummary(projectSummary));

    private static Result SetProjectSummary(ProjectSummary projectSummary)
    {
        var result = ProjectSummaryFileHelper.Save(projectSummary);
        if (result.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }

    public Task<Result> SetProjectAsync(Project project, CancellationToken ct = default) =>
        Task.FromResult(SetProject(project));

    private static Result SetProject(Project project)
    {
        var result = ProjectFileHelper.Save(project);
        if (result.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }
}