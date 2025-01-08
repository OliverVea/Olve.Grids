using Olve.Utilities.Operations;
using UI.Services.Projects.Repositories;

namespace UI.Services.Projects;

public class DeleteProjectOperation(IProjectDeletionRepository projectDeletionRepository)
    : IAsyncOperation<DeleteProjectOperation.Request,
        Result>
{
    public record Request(Id<Project> ProjectId);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var result = await projectDeletionRepository.DeleteProjectAsync(request.ProjectId, ct);
        if (result.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }
}