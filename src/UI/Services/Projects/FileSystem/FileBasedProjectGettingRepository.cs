using UI.Services.Projects.Repositories;

namespace UI.Services.Projects.FileSystem;

public class FileBasedProjectGettingRepository : IProjectGettingRepository
{
    public Task<Result<Project>> GetProjectAsync(Id<Project> projectId, CancellationToken ct = default)
        => Task.FromResult(GetProject(projectId));

    private static Result<Project> GetProject(Id<Project> projectId)
    {
        var projectFile = PathHelper.GetProjectPath(projectId);

        var result = ProjectFileHelper.Load(projectFile);
        if (result.TryPickProblems(out var problems, out var value))
        {
            return Result<Project>.Failure(problems);
        }

        return value;
    }

    public Task<Result<ProjectSummary>> GetProjectSummaryAsync(Id<Project> projectId, CancellationToken ct = default)
        => Task.FromResult(GetProjectSummary(projectId));

    private static Result<ProjectSummary> GetProjectSummary(Id<Project> projectId)
    {
        var summaryFile = PathHelper.GetSummaryPath(projectId);

        var result = ProjectSummaryFileHelper.Load(summaryFile);
        if (result.TryPickProblems(out var problems, out var value))
        {
            return Result<ProjectSummary>.Failure(problems);
        }

        return value;
    }

    public Task<Result<ProjectPath>> GetProjectPathAsync(Id<Project> projectId, CancellationToken ct = default)
        => Task.FromResult(GetProjectPath(projectId));

    private static Result<ProjectPath> GetProjectPath(Id<Project> projectId)
    {
        var path = PathHelper.GetProjectPath(projectId);

        return Path.Exists(path)
            ? new ProjectPath(path)
            : Result<ProjectPath>.Failure(new ResultProblem("Project path {0} does not exist.", path));
    }
}