using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.FileSystem;

public class FileBasedProjectDeletionRepository : IProjectDeletionRepository
{
    public Task<Result> DeleteProjectAsync(Id<Project> projectId, CancellationToken ct = default)
    {
        var result = DeleteProject(projectId);
        return Task.FromResult(result);
    }

    private Result DeleteProject(Id<Project> projectId)
    {
        var projectResult = ProjectFileHelper.Delete(projectId);
        if (projectResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        var projectSummaryResult = ProjectSummaryFileHelper.Delete(projectId);
        if (projectSummaryResult.TryPickProblems(out problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }
}