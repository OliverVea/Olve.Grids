using Olve.Utilities.Operations;
using UI.Services.Projects.Repositories;

namespace UI.Services.Projects;

public class CreateNewProjectOperation(
    IProjectSettingRepository projectSettingRepository,
    IProjectGettingRepository projectGettingRepository)
    : IAsyncOperation<
        CreateNewProjectOperation.Request,
        Result<CreateNewProjectOperation.Response>>
{
    public record Request(string Name);

    public record Response(ProjectSummary ProjectSummary);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var id = Id<Project>.NewId();
        var projectName = new ProjectName(request.Name);
        var createdAt = DateTimeOffset.Now;
        var lastAccessedAt = createdAt;

        var project = new Project(id, projectName, createdAt, lastAccessedAt);

        var createResult = await projectSettingRepository.SetProjectAsync(project, ct);
        if (createResult.TryPickProblems(out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        var projectPathResult = await projectGettingRepository.GetProjectPathAsync(id, ct);
        if (!projectPathResult.TryPickValue(out var projectPath, out problems))
        {
            return Result<Response>.Failure(problems);
        }

        var projectSummary = new ProjectSummary(id, projectName, projectPath, lastAccessedAt);

        var summaryResult = await projectSettingRepository.SetProjectSummaryAsync(projectSummary, ct);
        if (summaryResult.TryPickProblems(out problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(projectSummary);
    }
}